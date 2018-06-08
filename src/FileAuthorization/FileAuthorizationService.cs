using FileAuthorization.Abstractions;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace FileAuthorization
{
    public  class FileAuthorizationService : IFileAuthorizationService
    {

        
        public FileAuthorizationService(IOptions<FileAuthorizationOptions> options, IFileAuthorizationHandlerProvider provider)
        {
            Options = options;
            Provider = provider;
        }

        public IOptions<FileAuthorizationOptions> Options { get; }
        public IFileAuthorizationHandlerProvider Provider { get; }
    }
}
