using DirectoryService.Application.DTO;
using DirectoryService.Presentation.Dto;

namespace DirectoryService.Application.Interface;

public interface IProgramsService
{
    public Task<ProgramsResponseDto> GetProgramsAsync(ProgramsQueryDto queryDto);
    public Task<ProgramDto> GetProgramByIdAsync(Guid id);
}