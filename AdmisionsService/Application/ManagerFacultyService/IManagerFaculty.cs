using AdmisionsService.Application.Dtos;

namespace AdmisionsService.Application.ManagerFacultyService;

public interface IManagerFacultyService
{
    public Task CreateManagerFaculty(ManagerFacultyDto managerFaculty);
    public Task DeleteManagerFaculty(ManagerFacultyDto managerFaculty);
}