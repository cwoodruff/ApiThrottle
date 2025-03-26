using ApiThrottle.Services;
using Microsoft.AspNetCore.Http;

namespace ApiThrottle.Middleware;

public class ResponseThrottleMiddleware(RequestDelegate next, IThrottleService throttleService)
{
    public async Task InvokeAsync(HttpContext context)
    {
        await next(context);

        if (context.Response.StatusCode >= 200 && context.Response.StatusCode <= 299)
        {
            var apiKey = context.Request.Headers["X-Api-Key"].FirstOrDefault();
            if (!string.IsNullOrEmpty(apiKey))
            {
                await throttleService.LogRequestAsync(apiKey, true);
            }
        }
    }
}
