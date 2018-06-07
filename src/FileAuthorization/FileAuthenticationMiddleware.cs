using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace FileAuthorization
{
    public class FileAuthenticationMiddleware
    {
        private readonly RequestDelegate _next;
        public FileAuthenticationMiddleware(RequestDelegate next)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
        }

        public async Task Invoke(HttpContext context)
        {


            await _next(context);
        }
    }
}
