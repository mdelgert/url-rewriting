using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace RedirectManager;

public class RedirectMiddleware
{
    private readonly ILogger<RedirectMiddleware> _logger;
    private readonly RequestDelegate _next;

    // Constructor
    public RedirectMiddleware(RequestDelegate next, ILogger<RedirectMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Check if the request path should be redirected
        var path = context.Request.Path;
        var redirectData = RedirectService.GetRedirectData()
            .FirstOrDefault(r => path.Equals(r.RedirectUrl, StringComparison.OrdinalIgnoreCase));

        if (redirectData != null)
        {
            var targetUrl = redirectData.UseRelative
                ? new PathString(path.Value.Replace(redirectData.RedirectUrl, redirectData.TargetUrl))
                : new PathString(redirectData.TargetUrl);

            context.Response.Redirect(targetUrl, redirectData.UseRelative);
            _logger.LogInformation($"Redirected {path} to {targetUrl} ({redirectData.RedirectType}).");
            return;
        }

        await _next(context);
    }
}