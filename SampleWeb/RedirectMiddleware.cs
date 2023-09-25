using Microsoft.AspNetCore.Http;

namespace SampleWeb;

public class RedirectMiddleware
{
    private readonly RequestDelegate _next;
    private readonly RedirectService _redirectService;
    private readonly ILogger<RedirectMiddleware> _logger;

    // Constructor
    public RedirectMiddleware(RequestDelegate next, RedirectService redirectService, ILogger<RedirectMiddleware> logger)
    {
        _next = next;
        _redirectService = redirectService;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Check if the request path should be redirected
        var path = context.Request.Path;
        var redirectData = _redirectService.GetRedirectData()
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

