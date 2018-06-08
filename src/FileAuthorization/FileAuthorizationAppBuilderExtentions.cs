using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Text;

namespace FileAuthorization
{
    public static class FileAuthorizationAppBuilderExtentions
    {
        public static IApplicationBuilder UseFileAuthorization(this IApplicationBuilder app)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            return app.UseMiddleware<FileAuthenticationMiddleware>();
        }

    }
}
