using FileAuthorization.Abstractions;
using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.Text;

namespace FileAuthorization.Abstractions
{
    public class FileAuthorizationOptions
    {
        private List<FileAuthorizationScheme> _schemes = new List<FileAuthorizationScheme>(20);

        public string FileRootPath { get; set; }

        public string AuthorizationScheme { get; set; }

        public IEnumerable<FileAuthorizationScheme> Schemes { get => _schemes; }
        public void AddHandler<THandler>(string name) where THandler : IFileAuthorizeHandler
        {
            _schemes.Add(new FileAuthorizationScheme(name, typeof(THandler)));
        }
        public Type GetHandlerType(string scheme)
        {
            return _schemes.Find(s => s.Name == scheme)?.HandlerType;
        }
    }
}
