using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace FileAuthorization.Abstractions
{
    public interface IFileAuthorizeHandler
    {
        Task<FileAuthorizeResult> AuthorizeAsync(HttpContext context,string path);
    }
}