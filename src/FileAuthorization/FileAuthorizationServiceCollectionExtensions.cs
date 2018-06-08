using FileAuthorization.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace FileAuthorization
{
    public static class FileAuthorizationServiceCollectionExtensions
    {
        public static FileAuthorizationBuilder AddFileAuthorization(this IServiceCollection services)
        {
            return AddFileAuthorization(services, null);
        }

        public static FileAuthorizationBuilder AddFileAuthorization(this IServiceCollection services, Action<FileAuthorizationOptions> setup)
        {
            services.AddSingleton<IFileAuthorizationService, FileAuthorizationService>();
            services.AddSingleton<IFileAuthorizationHandlerProvider, FileAuthorizationHandlerProvider>();
            if (setup != null)
            {
                services.Configure(setup);
            }
            return new FileAuthorizationBuilder(services);
        }
    }
}
