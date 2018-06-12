using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace FileAuthorization.Abstractions
{
    public interface IFileAuthorizeHandler
    {

        /// <summary>
        /// Executing authorization for a request file.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="path">The requested file, Uri, begins with a slash,with out <see cref="FileAuthorizationOptions.AuthorizationScheme"/>, with out Handler scheme.</param>
        /// <returns></returns>
        Task<FileAuthorizeResult> AuthorizeAsync(HttpContext context,string path);
    }
}