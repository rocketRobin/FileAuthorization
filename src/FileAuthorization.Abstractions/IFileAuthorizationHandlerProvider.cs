using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace FileAuthorization.Abstractions
{
    public interface IFileAuthorizationHandlerProvider
    {

        /// <summary>
        /// Returns the handler instance that will be used.
        /// </summary>
        /// <param name="context">the http context.</param>
        /// <param name="scheme"> </param>
        /// <returns>The handler instance.</returns>
        Task<Type> GetHandlerAsync(HttpContext context, string scheme);

        bool Exist(string scheme);
    }
}
