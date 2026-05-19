namespace AdmisionsService.Domain;

public class Admission
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

    public int Priority { get; set; }

    public AdmisionStatus Status { get; set; } = AdmisionStatus.Open;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}