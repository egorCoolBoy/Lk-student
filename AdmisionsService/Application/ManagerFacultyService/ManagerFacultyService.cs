using AdmisionsService.Application.Dtos;
using AdmisionsService.Domain;
using AdmisionsService.Infrastructure.AppDbContext;

namespace AdmisionsService.Application.ManagerFacultyService;

public class ManagerFacultyService : IManagerFacultyService
{
    private readonly AppDbContext _db;

    public ManagerFacultyService(AppDbContext dbContext)
    {
        _db = dbContext;
    }

    public async Task CreateManagerFaculty(ManagerFacultyDto managerFaculty)
    {
        var managerFacultyy = new ManagerFaculty(managerFaculty.ManagerId, managerFaculty.FacultyId);
        await _db.AddAsync(managerFacultyy);
        await _db.SaveChangesAsync();
    }
    
    public async Task DeleteManagerFaculty(ManagerFacultyDto managerFaculty)
    {
        var managerFacultyy = await _db.FindAsync<ManagerFaculty>(managerFaculty.ManagerId);
        if (managerFacultyy == null)
            throw new KeyNotFoundException("Manager Faculty Not Found");
        _db.Remove(managerFacultyy);
        await _db.SaveChangesAsync();
    }
}