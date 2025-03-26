using ApiThrottle.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace ApiThrottle.Middleware;

public class RequestThrottleMiddleware(RequestDelegate next, IServiceProvider services)
{
    public async Task InvokeAsync(HttpContext context)
    {
        var throttleService = services.GetRequiredService<IThrottleService>();
        var apiKey = context.Request.Headers["X-Api-Key"].FirstOrDefault();

        if (string.IsNullOrEmpty(apiKey) || !(await throttleService.IncrementUsageAsync(apiKey)))
        {
            context.Response.StatusCode = StatusCodes.Status429TooManyRequests;
            await throttleService.LogRequestAsync(apiKey ?? "Unknown", false);
            return;
        }

        await next(context);
    }
}

