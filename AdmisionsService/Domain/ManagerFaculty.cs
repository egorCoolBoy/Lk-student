using AdmisionsService.Application.Dtos;

namespace AdmisionsService.Domain;

public class ManagerFaculty
{
    public Guid ManagerId { get; set; }
    public Guid FacultyId { get; set; }

    public ManagerFaculty(Guid managerId, Guid facultyId)
    {
        ManagerId = managerId;
        FacultyId = facultyId;
    }
}