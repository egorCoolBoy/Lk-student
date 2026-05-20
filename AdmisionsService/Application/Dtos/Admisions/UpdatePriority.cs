namespace AdmisionsService.Application.Dtos.Admisions;

public class UpdatePriorityDto
{
    public Guid ApplicantId { get; set; }
    public Guid ProgramId { get; set; }
    public int Priority { get; set; }
}