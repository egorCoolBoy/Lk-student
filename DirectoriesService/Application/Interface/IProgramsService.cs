using DirectoryService.Application.DTO;

namespace DirectoryService.Application.Interface;

public interface IProgramsService
{
    public Task<ProgramsResponseDto> GetProgramsAsync(int page, int size);
    public Task<ProgramDto> GetProgramByIdAsync(Guid id);
}