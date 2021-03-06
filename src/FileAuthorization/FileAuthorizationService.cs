﻿using FileAuthorization.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace FileAuthorization
{
    public class FileAuthorizationService : IFileAuthorizationService
    {
        private readonly ILogger<FileAuthorizationService> _logger;

        public FileAuthorizationService(IOptions<FileAuthorizationOptions> options, IFileAuthorizationHandlerProvider provider, ILogger<FileAuthorizationService> logger)
        {
            Options = options.Value;
            Provider = provider;
            _logger = logger;
        }

        public FileAuthorizationOptions  Options { get; }
        public IFileAuthorizationHandlerProvider Provider { get; }

        public string AuthorizationScheme => Options.AuthorizationScheme;

        public string FileRootPath => Options.FileRootPath;

        public async Task<FileAuthorizeResult> AuthorizeAsync(HttpContext context, string path)
        {
            var handlerScheme = GetHandlerScheme(path);
            if (handlerScheme == null || !Provider.Exist(handlerScheme))
            {
                _logger.LogInformation($"request handler scheme is not found. request path is: {path}");

                return FileAuthorizeResult.Fail();
            }

            var handlerType = Provider.GetHandlerType(handlerScheme);

            if (!(context.RequestServices.GetRequiredService(handlerType) is IFileAuthorizeHandler handler))
            {
                throw new Exception($"the required file authorization handler of '{handlerScheme}' is not found ");
            }

            // start with slash
            var requestFilePath = GetRequestFileUri(path, handlerScheme);
            return await handler.AuthorizeAsync(context, requestFilePath);
        }

        private string GetHandlerScheme(string path)
        {
            var arr = path.Split('/');
            if (arr.Length < 2)
            {
                return null;
            }

            // arr[0] is the Options.AuthorizationScheme
            return arr[1];
        }

        private string GetRequestFileUri(string path, string scheme)
        {
            return path.Remove(0, Options.AuthorizationScheme.Length + scheme.Length + 1);
        }
    }
}