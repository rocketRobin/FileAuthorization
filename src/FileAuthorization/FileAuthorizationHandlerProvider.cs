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
        private readonly  FileAuthorizationOptions  _options;

        /// <summary>
        /// Initialize <see cref="FileAuthorizationHandlerProvider"/>
        /// </summary>
        /// <param name="options"></param>
        public FileAuthorizationHandlerProvider(IOptions<FileAuthorizationOptions> options)
        {
            _options = options.Value;
        }

        /// <summary>
        /// Determine whether the provider contains this <paramref name="scheme"/>
        /// </summary>
        /// <param name="scheme">The name of the handler</param>
        /// <returns></returns>
        public bool Exist(string scheme)
        {
            return _options.Schemes.Any(c => c.Name == scheme);
        }

        /// <summary>
        /// Get the handler specified by the <paramref name="scheme"/>
        /// </summary>
        /// <param name="context"></param>
        /// <param name="scheme"></param>
        /// <returns></returns>
        public Type GetHandlerType(string scheme)
        {
            return _options.GetHandlerType(scheme);
        }
    }
}
