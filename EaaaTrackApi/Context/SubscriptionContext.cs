using EaaaTrackApi.Models;
using Microsoft.EntityFrameworkCore;

namespace EaaaTrackApi.Context;

public class SubscriptionContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Subscription> Subscriptions { get; set; }
    public DbSet<PaymentMethod> PaymentMethods { get; set; }
    public DbSet<SubscriptionCategory> Categories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Entity<Subscription>()
            .Property(s => s.BillingCycle)
            .HasConversion<string>();
    }
}