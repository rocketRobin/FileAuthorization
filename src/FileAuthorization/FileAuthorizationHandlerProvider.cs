using FileAuthorization.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileAuthorization
{
    public class FileAuthorizationHandlerProvider : IFileAuthorizationHandlerProvider
    {
        private readonly IOptions<FileAuthorizationOptions> _options;

        public FileAuthorizationHandlerProvider(IOptions<FileAuthorizationOptions> options)
        {
            _options = options;
        }

        public bool Exist(string scheme)
        {
          return  _options.Value.Schemes.Any(c => c.Name == scheme);
        }

        public Task<Type> GetHandlerAsync(HttpContext context, string scheme)
        {
            return Task.FromResult(_options.Value.GetHandlerType(scheme));
        }
    }
}
