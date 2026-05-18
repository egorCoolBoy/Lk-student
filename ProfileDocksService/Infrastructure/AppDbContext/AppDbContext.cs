using MassTransit;
using Microsoft.EntityFrameworkCore;
using ProfileDocksService.Domain.Entities;

namespace ProfileDocksService.Infrastructure.AppDbContext;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) :  base(options){}
    
    public DbSet<Profile> Profiles { get; set; }
    public DbSet<PassportDocument> PassportDocuments { get; set; }
    public DbSet<EducationDocument> EducationDocuments { get; set; }
    public DbSet<PassportScan> PassportScans { get; set; }
    public DbSet<EducationScan> EducationScans { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Profile>()
            .Property(x => x.UserId)
            .ValueGeneratedNever();
    }
}