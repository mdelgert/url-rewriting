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
        // Note caching is implemented, check if the request path should be redirected.
        var requestPath = context.Request.Path;
        if (requestPath.Value != null)
        {
            var pathSegments = requestPath.Value.Split(new[] {'/'}, StringSplitOptions.RemoveEmptyEntries);
            var productPath = "/" + pathSegments[^1];
            var redirectPath = requestPath.Value.Replace(productPath, "");

            var redirectData = RedirectService.GetRedirectData()
                .FirstOrDefault(r => redirectPath.Equals(r.RedirectUrl, StringComparison.OrdinalIgnoreCase));

            if (redirectData != null)
            {
                var targetUrl = redirectData.UseRelative
                    ? new PathString(requestPath.Value.Replace(redirectData.RedirectUrl, redirectData.TargetUrl))
                    : new PathString(redirectData.TargetUrl + productPath);

                // Returns a redirect response (HTTP 301 or HTTP 302) to the client.
                context.Response.Redirect(targetUrl, redirectData.RedirectType == 301);

                _logger.LogInformation($"Redirected {requestPath} to {targetUrl} ({redirectData.RedirectType}).");

                return;
            }
        }

        await _next(context);
    }
}