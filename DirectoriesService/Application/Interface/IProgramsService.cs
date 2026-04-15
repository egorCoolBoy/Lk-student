using DirectoryService.Application.DTO;

namespace DirectoryService.Application.Interface;

public interface IProgramsService
{
    public Task<List<ProgramDto>> GetProgramsAsync();
    public Task<ProgramDto> GetProgramByIdAsync(int id);
}