using Microsoft.AspNetCore.Hosting;
using System;
using System.Threading.Tasks;
using Xunit;
using FileAuthorization;
using FileAuthorization.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using System.IO;

namespace FileAuthorization.Test
{
    public class TestHandler : IFileAuthorizeHandler
    {
        public const string TestHandlerScheme = "id-card";

        public Task<FileAuthorizeResult> AuthorizeAsync(HttpContext context, string path)
        {
            return Task.FromResult(new FileAuthorizeResult(true, GetRelativeFilePath(path), GetDownloadFileName(path)));
        }

        public string GetRelativeFilePath(string path)
        {
            path = path.TrimStart('/', '\\').Replace('/', '\\');
            return $"{TestHandlerScheme}\\{path}";
        }

        public string GetDownloadFileName(string path)
        {
            return path.Substring(path.LastIndexOf('/') + 1);
        }
    }
    public class FileAuthenticationMiddlewareTest
    {
        [Fact]
        public async Task InvokeTest()
        {
            var builder = new WebHostBuilder()
                .Configure(app =>
                {
                    app.UseFileAuthorization();
                })
                .ConfigureServices(services =>
                {
                    services.AddFileAuthorization(options =>
                    {
                        options.AuthorizationScheme = "file";
                        options.FileRootPath = CreateFileRootPath();
                    })
                    .AddHandler<TestHandler>("id-card");
                });

            var server = new TestServer(builder);
            var response = await server.CreateClient().GetAsync("http://example.com/file/id-card/front.jpg");
            Assert.Equal(200, (int)response.StatusCode);
        }

        private string CreateFileRootPath()
        {
            var root = AppDomain.CurrentDomain.BaseDirectory;
            var index = root.IndexOf("bin");
            root = root.Remove(index);
            return Path.Combine(root, "Resource");
        }


    }
}
