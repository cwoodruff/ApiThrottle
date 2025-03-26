using ApiThrottle.Data;
using ApiThrottle.Middleware;
using ApiThrottle.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ApiThrottle.Extensions;

public static class ApiThrottleExtensions
{
    public static IServiceCollection AddApiThrottle(this IServiceCollection services, Action<DbContextOptionsBuilder> options)
    {
        services.AddDbContextPool<ApiThrottleDbContext>(options); // Use DbContext Pooling
        services.AddScoped<IThrottleService, ThrottleService>();
        return services;
    }


    public static IApplicationBuilder UseApiThrottle(this IApplicationBuilder app)
    {
        app.UseMiddleware<RequestThrottleMiddleware>();
        app.UseMiddleware<ResponseThrottleMiddleware>();
        return app;
    }
}
