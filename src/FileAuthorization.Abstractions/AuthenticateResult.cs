using System;

namespace FileAuthorization.Abstractions
{
    public class FileAuthorizeResult
    {
        private FileAuthorizeResult(bool succeeded, string relativePath, string fileDownloadName, Exception failure = null)
        {
            Succeeded = succeeded;
            RelativePath = relativePath;
            FileDownloadName = fileDownloadName;
            Failure = failure;
        }
        public bool Succeeded { get; }


        /// <summary>
        /// File path relative to <see cref="FileAuthorizationOptions.FileRootPath"/> (including file name).
        /// </summary>
        /// <remarks>
        /// strart with out slash
        /// </remarks>
        public string RelativePath { get; }

        /// <summary>
        /// File name downloaded to the client.
        /// </summary>
        public string FileDownloadName { get; set; }
        public Exception Failure { get; }

        public static FileAuthorizeResult Success(string relativePath, string fileDownloadName)
        {
            return new FileAuthorizeResult(true, relativePath, fileDownloadName, null);
        }
        public static FileAuthorizeResult Fail(Exception failure = null)
        {
            return new FileAuthorizeResult(false, null, null, failure);
        }

    }
}