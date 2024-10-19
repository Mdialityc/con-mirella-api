using ConMirellaApi.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace ConMirellaApi.Data;

public class AppDbContext: DbContext
{
    public AppDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Group> Groups => Set<Group>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Platform> Platforms => Set<Platform>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Valuate> Valuates => Set<Valuate>();
    public DbSet<SearchLog> SearchLogs => Set<SearchLog>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // Group
        modelBuilder.Entity<Platform>()
            .HasMany<Group>()
            .WithOne()
            .HasForeignKey(e => e.PlatformId)
            .IsRequired();
        modelBuilder.Entity<Category>()
            .HasMany<Group>()
            .WithOne()
            .HasForeignKey(e => e.CategoryId)
            .IsRequired();
        
        // Valuate
        modelBuilder.Entity<Group>()
            .HasMany<Valuate>()
            .WithOne()
            .HasForeignKey(e => e.GroupId)
            .IsRequired();
    }
}