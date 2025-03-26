using ApiThrottle.Models;

namespace ApiThrottle.Services;

public interface IThrottleService
{
    Task<Customer?> GetCustomerAsync(string apiKey);
    Task<bool> IncrementUsageAsync(string apiKey);
    Task LogRequestAsync(string apiKey, bool isSuccess);
}
