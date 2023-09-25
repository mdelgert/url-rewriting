using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;

namespace RedirectManager;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        // Add caching
        services.AddMemoryCache();

        // Add services and other configurations
        services.AddSingleton<RedirectService>();

        // ...
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        // Configure cache duration (e.g., 5 minutes)
        //var cacheDuration = TimeSpan.FromMinutes(Configuration.GetValue<int>("RedirectCacheDuration", 5));
        var cacheDuration = TimeSpan.FromMinutes(1);

        app.UseMiddleware<RedirectMiddleware>();
        app.UseResponseCaching();

        // Configure caching
        app.Use(async (context, next) =>
        {
            context.Response.GetTypedHeaders().CacheControl = new CacheControlHeaderValue
            {
                MaxAge = cacheDuration,
                Public = true
            };
            context.Response.Headers[HeaderNames.Vary] = new[] { "Accept-Encoding" };

            await next();
        });
    }
}

