using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using FileAuthorization.Abstractions;

namespace FileAuthorization
{
    public class FileAuthenticationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string _pathBase;
        public FileAuthenticationMiddleware(RequestDelegate next)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
        }

        public async Task Invoke(HttpContext context)
        {
            var path = context.Request.Path.Value;
            var scheme = GetScheme(path);

            if (!BelongToMe(path))
            {
                await _next.Invoke(context);
                return;
            }
            var handlers = context.RequestServices.GetRequiredService<IFileAuthenticationHandlerProvider>();
            var handler = await handlers.GetHandlerAsync(context, scheme);

            if (handler == null)
            {
                return;
            }

            var requestFilePath = GetRequestFilePath(path, scheme);
            var result = await handler.AuthenticateAsync(context, requestFilePath);




            if (!result.Succeeded)
            {
                context.Response.StatusCode = 403;
                return;
            }

            using (var stream=)
            {

            }


        }

        private static string GetRequestFilePath(string path, string scheme)
        {

            return path.Remove(0, path.Length + scheme.Length + 1);
        }

        private bool BelongToMe(string path)
        {
            return path.StartsWith(_pathBase);
        }

        private string GetScheme(string path)
        {
            return path.Split('/')[1];
        }
    }
}
