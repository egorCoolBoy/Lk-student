using AdmisionsService.Application.Dtos;

namespace AdmisionsService.Application.Interfaces;

public interface IDocumentAPI
{
    public Task<List<EducationDocxDto>> GetDocxAsync(Guid userId);
    public Task<GetProfileDto> GetProfileAsync(Guid userId);
}