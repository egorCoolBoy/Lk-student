using AdmisionsService.Application.Dtos;

namespace AdmisionsService.Application.Interfaces;

public interface IDirectoriesAPI
{
    public Task<ProgramDto> GetProgramByIdAsync(Guid id);
}