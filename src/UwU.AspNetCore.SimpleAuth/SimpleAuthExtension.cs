using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using static UwU.AspNetCore.SimpleAuth.SecretKeyProvider;

namespace UwU.AspNetCore.SimpleAuth
{
    public static class SimpleAuthExtension
    {
        public static void AddSimpleAuth(this IHostApplicationBuilder builder, string configKey, CompareSafeDelegate comparator)
        {
            var secret = builder.Configuration[configKey] ?? throw new InvalidOperationException($"{configKey} not set on appsettings.json");
            builder.Services.AddSingleton(new SecretKeyProvider(secret, comparator));
        }

        public static void UseSimpleAuth(this IApplicationBuilder app, string[] entries = null)
        {
            if (entries == null)
            {
                app.UseMiddleware<SecretKeyAuthMiddleware>();   // Apply to all
            }
            else
            {
                foreach (var pattern in entries)
                {
                    app.UseWhen(
                        ctx => ctx.Request.Path.StartsWithSegments($"/{pattern}"),
                        branch => branch.UseMiddleware<SecretKeyAuthMiddleware>()
                    );
                }
            }
        }
    }
}
