namespace WebApplication1.Models;

public class ImportedDirectroriesStatistic
{
    public Guid Id { get; set; }
    public ImportedDirectories Imported { get; set; } = new();
    public DateTime ImportTime { get; set; }
}

public class ImportedDirectories
{
    public int EducationLevels { get; set; }
    
    public int DocumentTypes { get; set; }
    
    public int Faculties { get; set; }
    
    public int Programs { get; set; }
}