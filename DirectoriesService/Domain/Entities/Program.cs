namespace DirectoryService.Domain.Entities;

public class Program
{
    public Guid Id { get; set; }
    public DateTime CreateTime { get; set; }

    public string Name { get; set; } = null!;
    public string Code { get; set; } = null!;

    public string Language { get; set; } = null!;
    public string EducationForm { get; set; } = null!;
    
    public Guid FacultyId { get; set; }
    public Faculty Faculty { get; set; } = null!;

    public int EducationLevelId { get; set; }
    public EducationLevel EducationLevel { get; set; } = null!;
}