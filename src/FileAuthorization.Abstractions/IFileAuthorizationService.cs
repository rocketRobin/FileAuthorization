using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace FileAuthorization.Abstractions
{
    public interface IFileAuthorizationService
    {
        string AuthorizationScheme { get; }
        string FileRootPath { get; }
        
        //IFileAuthorizationHandlerProvider Provider { get; }

        /// <summary>
        /// Authorizing the requested URI.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="path">The request path start with out shlash.</param>
        /// <returns></returns>
        Task<FileAuthorizeResult> AuthorizeAsync(HttpContext context, string path);
    }
}