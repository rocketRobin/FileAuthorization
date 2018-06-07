using System;
using System.Collections.Generic;
using System.Text;

namespace FileAuthorization.Abstractions
{
    public interface IFileAuthenticationSchemeProvider
    {
        void AddScheme(string scheme);
        IEnumerable<string> GetAllSchemes();
        bool Exist(string name);
        void RemoveScheme(string name);
    }
}
