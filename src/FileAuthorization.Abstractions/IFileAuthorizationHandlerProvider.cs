using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace FileAuthorization.Abstractions
{
    public interface IFileAuthorizationHandlerProvider
    {

        /// <summary>
        /// Returns the handler type that will be used.
        /// </summary>
        /// <param name="scheme"> </param>
        /// <returns>The handler instance.</returns>
        Type GetHandlerType (string scheme);

        bool Exist(string scheme);
    }
}
