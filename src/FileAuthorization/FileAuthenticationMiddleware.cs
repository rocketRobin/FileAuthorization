using FileAuthorization.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Myrmec;
using System;
using System.Globalization;
using System.IO;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace FileAuthorization
{
    public class FileAuthenticationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<FileAuthenticationMiddleware> _logger;

        public FileAuthenticationMiddleware(RequestDelegate next, IFileAuthorizationService service, ILogger<FileAuthenticationMiddleware> logger)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _service = service;
            _logger = logger;
        }

        private readonly IFileAuthorizationService _service;
        public int BufferSize = 64 * 1024;

        public async Task Invoke(HttpContext context)
        {
            // trim the start slash
            var path = context.Request.Path.Value.TrimStart('/');

            if (!BelongToMe(path))
            {
                await _next.Invoke(context);
                return;
            }

            var handlerScheme = GetHandlerScheme(path);
            if (handlerScheme == null || !_service.Provider.Exist(handlerScheme))
            {
                _logger.LogInformation($"request handler scheme is not found. request path is: {path}");
                NotFound(context);
                return;
            }

            var handlerType =   _service.Provider.GetHandlerType( handlerScheme);


            if (!(context.RequestServices.GetRequiredService(handlerType) is IFileAuthorizeHandler handler))
            {
                throw new Exception($"the required file authorization handler of '{handlerScheme}' is not found ");
            }

            // start with slash
            var requestFilePath = GetRequestFileUri(path, handlerScheme);
            var result = await handler.AuthorizeAsync(context, requestFilePath);

            if (!result.Succeeded)
            {
                _logger.LogInformation($"request file is forbidden. request path is: {path}");
                Forbidden(context);
                return;
            }

            if (string.IsNullOrWhiteSpace(_service.Options.Value.FileRootPath))
            {
                throw new Exception("file root path is not spicificated");
            }

            string fullName;

            if (Path.IsPathRooted(result.RelativePath))
            {
                fullName = result.RelativePath;
            }
            else
            {
                fullName = Path.Combine(_service.Options.Value.FileRootPath, result.RelativePath);
            }
            var fileInfo = new FileInfo(fullName);

            if (!fileInfo.Exists)
            {
                NotFound(context);
                return;
            }

            _logger.LogInformation($"{context.User.Identity.Name} request file :{fileInfo.FullName} has beeb authorized. File sending");
            await SetResponseHeadersAndWriteFileAsync(context, result, fileInfo);

        }

        private string GetRequestFileUri(string path, string scheme)
        {
            return path.Remove(0, _service.Options.Value.AuthorizationScheme.Length + scheme.Length + 1);
        }

        private bool BelongToMe(string path)
        {
            return path.StartsWith(_service.Options.Value.AuthorizationScheme, true, CultureInfo.CurrentCulture);
        }

        private string GetHandlerScheme(string path)
        {
            var arr = path.Split('/');
            if (arr.Length < 2)
            {
                return null;
            }
            return arr[1];
        }

        private async Task SetResponseHeadersAndWriteFileAsync(HttpContext context, FileAuthorizeResult result, FileInfo fileInfo)
        {
            var response = context.Response;

            response.ContentType = GetContentType(fileInfo);
            SetContentDispositionHeader(context, result);
            var heders = response.GetTypedHeaders();

            heders.LastModified = fileInfo.LastWriteTimeUtc;
            var sendFile = response.HttpContext.Features.Get<IHttpSendFileFeature>();
            if (sendFile != null)
            {
                await sendFile.SendFileAsync(fileInfo.FullName, 0L, null, default(CancellationToken));
                return;
            }

            var outputStream = context.Response.Body;
            using (var fileStream = new FileStream(
                    fileInfo.FullName,
                    FileMode.Open,
                    FileAccess.Read,
                    FileShare.ReadWrite,
                    BufferSize,
                    FileOptions.Asynchronous | FileOptions.SequentialScan))
            {
                try
                {

                    await StreamCopyOperation.CopyToAsync(fileStream, outputStream, count: null, bufferSize: BufferSize, cancel: context.RequestAborted);

                }
                catch (OperationCanceledException)
                {
                    // Don't throw this exception, it's most likely caused by the client disconnecting.
                    // However, if it was cancelled for any other reason we need to prevent empty responses.
                    context.Abort();
                }
            }

        }

        private void SetContentDispositionHeader(HttpContext context, FileAuthorizeResult result)
        {
            if (!string.IsNullOrEmpty(result.FileDownloadName))
            {
                // From RFC 2183, Sec. 2.3:
                // The sender may want to suggest a filename to be used if the entity is
                // detached and stored in a separate file. If the receiving MUA writes
                // the entity to a file, the suggested filename should be used as a
                // basis for the actual filename, where possible.
                var contentDisposition = new ContentDispositionHeaderValue("attachment")
                {
                    FileName = result.FileDownloadName
                };
                context.Response.Headers["Content-Disposition"] = contentDisposition.ToString();
            }
        }

        private void NotFound(HttpContext context)
        {
            HttpCode(context, 404);
        }
        private void Forbidden(HttpContext context)
        {
            HttpCode(context, 403);
        }

        private void HttpCode(HttpContext context, int code)
        {
            context.Response.StatusCode = code;
        }


        private string GetContentType(FileInfo fileInfo)
        {
            return MimeTypes.GetMimeType(fileInfo.Extension);
        }
    }
}