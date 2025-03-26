namespace ApiThrottle.Models;

public class ApiUsageLog
{
    public int Id { get; set; }
    public string? ApiKey { get; set; }
    public bool IsSuccess { get; set; }
    public DateTime Timestamp { get; set; }
}
