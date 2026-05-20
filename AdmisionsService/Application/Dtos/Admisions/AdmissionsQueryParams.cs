using AdmisionsService.Domain;

namespace AdmisionsService.Application.Dtos.Admisions;

public class AdmissionsQueryParams
{
    public int Page { get; set; } = 1;

    public int PageSize { get; set; } = 10;

    public string? Search { get; set; }

    public Guid? ProgramId { get; set; }

    public List<Guid>? FacultyIds { get; set; }

    public AdmisionStatus? Status { get; set; }

    public bool OnlyWithoutManager { get; set; }

    public Guid? ManagerId { get; set; }

    public string? SortByUpdatedAt { get; set; } = "desc";
}