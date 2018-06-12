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


        /// <summary>
        /// The root directory of the application store file.
        /// </summary>
        public string FileRootPath { get; set; }

        /// <summary>
        /// The first section of the HTTP request path.
        /// </summary>
        public string AuthorizationScheme { get; set; }

        /// <summary>
        /// Registered file authorization handler collection.
        /// </summary>
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
