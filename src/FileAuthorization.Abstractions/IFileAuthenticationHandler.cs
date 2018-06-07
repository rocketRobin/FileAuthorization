using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace FileAuthorization.Abstractions
{
    public interface IFileAuthenticationHandler
    {
        Task<AuthenticateResult> AuthenticateAsync(HttpContext context,string path);
    }
}