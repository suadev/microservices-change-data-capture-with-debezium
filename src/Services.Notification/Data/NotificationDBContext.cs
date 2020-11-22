using Microsoft.EntityFrameworkCore;

namespace Services.Notification.Data
{
    public class NotificationDBContext : DbContext
    {
        public NotificationDBContext(DbContextOptions<NotificationDBContext> options) : base(options)
        {
        }
        public DbSet<Customer> Customers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("notification");
            modelBuilder.Entity<Customer>().ToTable("customers");
        }
    }
}