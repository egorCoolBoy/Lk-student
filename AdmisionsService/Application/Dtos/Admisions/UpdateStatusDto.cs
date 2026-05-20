using AdmisionsService.Domain;

namespace AdmisionsService.Application.Dtos.Admisions;

public class UpdateStatusDto
{
    public Guid ApplicantId { get; set; }
    public Guid ProgramId { get; set; }
    public AdmisionStatus Status { get; set; }
}