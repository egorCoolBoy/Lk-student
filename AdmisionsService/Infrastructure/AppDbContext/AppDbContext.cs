using Microsoft.EntityFrameworkCore;
using AdmisionsService.Domain;
using MassTransit;

namespace AdmisionsService.Infrastructure.AppDbContext;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {}
    public DbSet<Admission> Admissions { get; set; }
    public DbSet<ManagerFaculty>  ManagerFaculties { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Admission>(entity =>
        {
            entity.HasKey(x => new { x.ApplicantId, x.ProgramId });
        });
        
        modelBuilder.Entity<ManagerFaculty>(entity =>
        {
            entity.HasKey(x => new { x.ManagerId, x.FacultyId });
        });
        modelBuilder.AddOutboxStateEntity();
        modelBuilder.AddOutboxMessageEntity();
        modelBuilder.AddInboxStateEntity();
    }
    
    
}