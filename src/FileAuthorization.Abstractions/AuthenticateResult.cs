using System;

namespace FileAuthorization.Abstractions
{
    public class FileAuthorizeResult
    {
        public FileAuthorizeResult(bool succeeded,string relativePath,string fileDownloadName,Exception failure=null)
        {
            Succeeded = succeeded;
            RelativePath = relativePath;
            FileDownloadName = fileDownloadName;
            Failure = failure;
        }
        public bool Succeeded { get; }
        public string RelativePath { get; }
        public string FileDownloadName { get; set; }
        public Exception Failure { get; }
    }
}