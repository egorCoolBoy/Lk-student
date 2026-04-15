namespace DirectoryService.Domain.Entities;

public class EducationDocument
{
    public Guid Id { get; set; }
    public DateTime CreateTime { get; set; }

    public string Name { get; set; } = null!;
    public int EducationLevelId { get; set; }
    
    public List<EducationDocumentNextLevel> NextLevels { get; set; } = new();
}