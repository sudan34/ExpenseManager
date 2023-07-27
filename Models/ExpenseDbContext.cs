using Microsoft.EntityFrameworkCore;

namespace ExpenseManager.Models
{
    public class ExpenseDbContext : DbContext
    {
        public ExpenseDbContext(DbContextOptions<ExpenseDbContext> options) : base(options)
        {
        }
        public DbSet<ExpenseReport> ExpenseReports { get; set; }
    }
}
