using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;

namespace RedirectManager;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    private IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        // Add caching
        services.AddMemoryCache();

        // Add services and other configurations
        services.AddSingleton<RedirectService>();

        // Add configuration to the DI container
        services.AddSingleton(Configuration);
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        // Configure cache duration from app settings, if no value defaults to 5 minutes
        var cacheDuration = TimeSpan.FromMinutes(Configuration.GetValue("RedirectCacheDuration", 5));

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
            context.Response.Headers[HeaderNames.Vary] = new[] {"Accept-Encoding"};

            await next();
        });
    }
}