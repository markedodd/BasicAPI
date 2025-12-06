using Microsoft.EntityFrameworkCore;
using BasicApp.Models.Entities;

namespace BasicApp.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Hook for future entity configurations (can use reflection here too)
        base.OnModelCreating(modelBuilder);
    }
}
