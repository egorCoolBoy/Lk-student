using DirectoryService.Application.DTO;
namespace DirectoryService.Application.Interface;

public interface IEducationLevelService
{
    public Task<List<EducationDocumentDto>> GetEducationDocumentsAsync();
    public Task<EducationDocumentDto> GetEducationDocumentsByIdAsync(int id);
}