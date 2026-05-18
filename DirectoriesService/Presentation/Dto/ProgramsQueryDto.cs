namespace DirectoryService.Presentation.Dto;

public class ProgramsQueryDto
{
    public int Page { get; set; } = 1;
    public int Size { get; set; } = 10;

    public Guid? FacultyId { get; set; }
    public int? EducationLevelId { get; set; }

    public string? EducationForm { get; set; }
    public string? Language { get; set; }
    
    public string? Search { get; set; }
}