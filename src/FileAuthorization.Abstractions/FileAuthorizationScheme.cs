using System;
using System.Collections.Generic;
using System.Text;

namespace FileAuthorization.Abstractions
{
    public class FileAuthorizationScheme
    {
        public FileAuthorizationScheme(string name, Type handlerType)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("message", nameof(name));
            }

            Name = name;
            HandlerType = handlerType ?? throw new ArgumentNullException(nameof(handlerType));
        }
        public string Name { get; }
        public Type HandlerType { get; }
    }
}
