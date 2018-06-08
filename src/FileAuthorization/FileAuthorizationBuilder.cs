using FileAuthorization.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace FileAuthorization
{
    public class FileAuthorizationBuilder
    {
        public FileAuthorizationBuilder(IServiceCollection services)
        {
            Services = services;
        }

        public IServiceCollection Services { get; }

        public FileAuthorizationBuilder AddHandler<THandler>(string name) where THandler : class, IFileAuthorizeHandler
        {
            Services.Configure<FileAuthorizationOptions>(options =>
            {
                options.AddHandler<THandler>(name );
            });

            Services.AddTransient<THandler>();
            return this;
        }

       
    }
}
