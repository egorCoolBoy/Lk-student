using ProfileDocksService.Applicantion.Dtos;

namespace ProfileDocksService.Applicantion.Interface;

public interface IDirectoriesAPI
{
    public Task<EducationLevelDto> GetEducationLevel(int n);
    public Task<EducationDocumentTypesDto> GetEducationDocumentTypeByIdAsync(Guid id);
}