namespace DirectoryService.Domain.Entities;

public class EducationDocumentNextLevel
{
    public Guid EducationDocumentId { get; set; }
    public EducationDocument EducationDocument { get; set; } = null!;

    public int EducationLevelId { get; set; }
    public EducationLevel EducationLevel { get; set; } = null!;
}