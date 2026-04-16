using DirectoryService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DirectoryService.Infrastructure.AppDbContext;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options){}
    
    public DbSet<EducationDocumentNextLevel> EducationDocumentNextLevels { get; set; }
    public DbSet<EducationLevel> EducationLevels { get; set; }
    public DbSet<Faculty> Faculties { get; set; } 
    public DbSet<DirectoryService.Domain.Entities.Program> Programs { get; set; }  
    public DbSet<EducationDocument>EducationDocuments { get; set; }
    public DbSet<ImportedDirectroriesStatistic> DirectoryImportStatistics { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<EducationDocumentNextLevel>()
            .HasKey(x => new { x.EducationDocumentId, x.EducationLevelId });

        modelBuilder.Entity<ImportedDirectroriesStatistic>()
            .OwnsOne(x => x.Imported);

        modelBuilder.Entity<EducationDocumentNextLevel>()
            .HasOne(x => x.EducationDocument)
            .WithMany(x => x.NextLevels)
            .HasForeignKey(x => x.EducationDocumentId);

        modelBuilder.Entity<EducationDocumentNextLevel>()
            .HasOne(x => x.EducationLevel)
            .WithMany(x => x.DocumentLinks)
            .HasForeignKey(x => x.EducationLevelId);
    }
}