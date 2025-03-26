using ApiThrottle.Data;
using ApiThrottle.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ApiThrottle.Services;

public class ThrottleService(ApiThrottleDbContext context, ILogger<ThrottleService> logger)
    : IThrottleService
{
    public async Task<Customer?> GetCustomerAsync(string apiKey)
    {
        logger.LogDebug("Fetching customer with API key: {ApiKey}", apiKey);
        return await context.Customers.FirstOrDefaultAsync(c => c.ApiKey == apiKey);
    }

    public async Task<bool> IncrementUsageAsync(string apiKey)
    {
        var customer = await GetCustomerAsync(apiKey);
        if (customer == null)
        {
            logger.LogWarning("No customer found for API key: {ApiKey}", apiKey);
            return false;
        }

        if (DateTime.UtcNow > customer.PeriodStart.Add(customer.Period))
        {
            customer.PeriodStart = DateTime.UtcNow;
            customer.UsedRequests = 0;
        }

        if (customer.UsedRequests >= customer.MaxRequests)
        {
            logger.LogWarning("Customer {ApiKey} exceeded request limit.", apiKey);
            return false;
        }
        
        return true;
    }

    public async Task LogRequestAsync(string apiKey, bool isSuccess)
    {
        var customer = await GetCustomerAsync(apiKey);
        if (customer == null)
        {
            logger.LogWarning("No customer found for API key: {ApiKey}", apiKey);
            return;
        }
        
        logger.LogInformation("Logging request for API key: {ApiKey}, Success: {IsSuccess}", apiKey, isSuccess);

        var log = new ApiUsageLog
        {
            ApiKey = apiKey,
            IsSuccess = isSuccess,
            Timestamp = DateTime.UtcNow
        };
        context.ApiUsageLogs.Add(log);
        customer.UsedRequests++;
        await context.SaveChangesAsync();
    }
}