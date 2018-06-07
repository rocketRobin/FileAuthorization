using System;

namespace FileAuthorization.Abstractions
{
    public class AuthenticateResult
    {
        public bool Succeeded { get; }
        public string Path { get; }
        public bool None { get; }
        public Exception Failure { get; }
    }
}