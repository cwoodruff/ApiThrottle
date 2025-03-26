namespace ApiThrottle.Models;

public class Customer
{
    public int Id { get; set; }
    public string? ApiKey { get; set; }
    public int MaxRequests { get; set; }
    public TimeSpan Period { get; set; }
    public int UsedRequests { get; set; }
    public DateTime PeriodStart { get; set; }
}
