namespace AdmisionsService.Application.Dtos.Admisions;

public class ManagerAdmission
{
    public Guid ApplicantId { get; set; }
    public Guid ProgramId { get; set; }
    public Guid ManagerId { get; set; }
}