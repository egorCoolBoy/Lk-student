namespace ProfileDocksService.Applicantion.Dtos;

public class EducationDocumentTypesDto
{
    public Guid Id { get; set; }
    public DateTime CreateTime { get; set; }

    public string Name { get; set; } = null!;

    public EducationLevelDto EducationLevel { get; set; } = null!;
    public List<EducationLevelDto> NextEducationLevels { get; set; } = new();
}