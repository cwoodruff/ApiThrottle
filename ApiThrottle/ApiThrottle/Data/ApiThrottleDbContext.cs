using ApiThrottle.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiThrottle.Data
{
    public class ApiThrottleDbContext(DbContextOptions<ApiThrottleDbContext> options) : DbContext(options)
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<ApiUsageLog> ApiUsageLogs { get; set; }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

        }
    }
}
