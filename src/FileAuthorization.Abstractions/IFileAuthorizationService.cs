using FileAuthorization.Abstractions;
using Microsoft.Extensions.Options;

namespace FileAuthorization.Abstractions
{
    public interface IFileAuthorizationService
    {
        IOptions<FileAuthorizationOptions> Options { get; }
        IFileAuthorizationHandlerProvider Provider { get; }
    }
}