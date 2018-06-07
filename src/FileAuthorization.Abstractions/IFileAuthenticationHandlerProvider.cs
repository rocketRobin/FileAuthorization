using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace FileAuthorization.Abstractions
{
    public  interface IFileAuthenticationHandlerProvider
    {

        /// <summary>
        /// Returns the handler instance that will be used.
        /// </summary>
        /// <param name="context">the http context.</param>
        /// <param name="authenticationScheme">The name of the authentication scheme being handled.</param>
        /// <returns>The handler instance.</returns>
         Task<IFileAuthenticationHandler> GetHandlerAsync(HttpContext context, string authenticationScheme);
    }
}
