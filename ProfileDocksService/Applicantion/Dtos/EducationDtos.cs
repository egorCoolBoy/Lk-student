using ProfileDocksService.Domain.Enums;

namespace ProfileDocksService.Applicantion.Dtos;

public class CreateEducationDto
{
    public string InstitutionName { get; set; }
    public EducationLevel Level { get; set; }
    public string Specialty { get; set; }
    public string GraduationDate { get; set; }
    public string DiplomaNumber { get; set; }
}

public class GetEducationDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string InstitutionName { get; set; }
    public EducationLevel Level { get; set; }
    public string Specialty { get; set; }
    public DateOnly GraduationDate { get; set; }
    public string DiplomaNumber { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class UpdateEducationDto
{
    public string? InstitutionName { get; set; }
    public EducationLevel? Level { get; set; }
    public string? Specialty { get; set; }
    public string? GraduationDate { get; set; }
    public string? DiplomaNumber { get; set; }
}