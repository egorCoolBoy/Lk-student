using MassTransit;
using MassTransit.EntityFrameworkCoreIntegration;
using Microsoft.EntityFrameworkCore;
using UsersService.Domain.Entities;

namespace UsersService.Infrastructure.AppDbContext;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options){}
    public DbSet<User> Users { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
 
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RefreshToken>()
            .HasOne<User>()
            .WithMany(u => u.RefreshTokens)
            .HasForeignKey(rt => rt.UserId);
        
        modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();
        
        modelBuilder.AddOutboxStateEntity();
        modelBuilder.AddOutboxMessageEntity();
        modelBuilder.AddInboxStateEntity();
    }
}