namespace AdmisionsService.Application.Dtos;

public class EducationDocxDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string? InstitutionName { get; set; }
    public string? Level { get; set; }
    public string? Specialty { get; set; }
    public DateOnly? GraduationDate { get; set; }
    public string? DiplomaNumber { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}