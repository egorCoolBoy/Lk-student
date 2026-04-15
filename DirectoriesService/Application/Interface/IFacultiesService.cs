using DirectoryService.Application.DTO;

namespace DirectoryService.Application.Interface;

public interface IFacultiesService
{
    public Task<List<FacultyDto>> GetFacultiesAsync();
    public Task<FacultyDto> GetFacultyByIdAsync(Guid id);
}