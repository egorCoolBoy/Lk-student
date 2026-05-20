namespace AdmisionsService.Application.Dtos.Admisions;

public class GetAdmissionByIds
{
    public Guid ApplicantId { get; set; }
    public Guid ProgramId { get; set; }
}