using AdmisionsService.Domain;

namespace AdmisionsService.Application.Dtos.Admisions;

public class GetAdmissionDto
{
    public Guid ApplicantId { get; set; }
    public string ApplicantFullName { get; set; } = null!;

    public Guid ProgramId { get; set; }
    public string ProgramName { get; set; } = null!;

    public Guid FacultyId { get; set; }
    public string FacultyName { get; set; } = null!;

    public Guid? ManagerId { get; set; }
    public string? ManagerFullName { get; set; }

    public string EducationLevel { get; set; } = null!;
    public int EducationLevelId { get; set; }

    public int Priority { get; set; }

    public AdmisionStatus Status { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}