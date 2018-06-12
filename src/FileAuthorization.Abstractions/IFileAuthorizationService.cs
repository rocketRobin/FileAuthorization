using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace FileAuthorization.Abstractions
{
    public interface IFileAuthorizationService
    {
        FileAuthorizationOptions  Options { get; }
        IFileAuthorizationHandlerProvider Provider { get; }
        Task<FileAuthorizeResult> AuthorizeAsync(HttpContext context, string path);
    }
}