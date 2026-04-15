namespace DirectoryService.Domain.Entities;

public class EducationLevel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<EducationDocumentNextLevel> DocumentLinks { get; set; } = new();
}