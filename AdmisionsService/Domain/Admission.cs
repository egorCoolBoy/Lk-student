namespace AdmisionsService.Domain;

public class Admission
{
    public Guid ApplicantId { get; set; }
    public string ApplicantFullName { get; set; } 


    public Guid ProgramId { get; set; }
    public string ProgramName { get; set; } 


    public Guid FacultyId { get; set; }
    public string FacultyName { get; set; }


    public Guid? ManagerId { get; set; }
    public string? ManagerFullName { get; set; }
    
    public string EducationLevel { get; set; } 
    public int EducationLevelId { get; set; } 

    public int Priority { get; set; }

    public AdmisionStatus Status { get; set; } = AdmisionStatus.Open;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } 
}