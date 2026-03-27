using Himchistka.Core.Models;
using Himchistka.Data.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Himchistka.Data.Database;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Service> Services => Set<Service>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();
    public DbSet<Payment> Payments => Set<Payment>();
    public DbSet<Discount> Discounts => Set<Discount>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new OrderConfiguration());
        modelBuilder.ApplyConfiguration(new OrderItemConfiguration());
        modelBuilder.ApplyConfiguration(new PaymentConfiguration());

        modelBuilder.Entity<Order>().Property(x => x.TotalAmount).HasColumnType("decimal(18,2)");
        modelBuilder.Entity<Order>().Property(x => x.PaidAmount).HasColumnType("decimal(18,2)");
        modelBuilder.Entity<OrderItem>().Property(x => x.UnitPrice).HasColumnType("decimal(18,2)");
        modelBuilder.Entity<OrderItem>().Property(x => x.UrgentExtra).HasColumnType("decimal(18,2)");
        modelBuilder.Entity<Service>().Property(x => x.BasePrice).HasColumnType("decimal(18,2)");
        modelBuilder.Entity<Payment>().Property(x => x.Amount).HasColumnType("decimal(18,2)");
    }
}
