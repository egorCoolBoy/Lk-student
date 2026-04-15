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
}