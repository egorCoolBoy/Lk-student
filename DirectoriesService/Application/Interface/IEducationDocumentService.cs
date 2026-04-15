using DirectoryService.Application.DTO;

namespace DirectoryService.Application.Interface;

public interface IEducationDocumentService
{
    public Task<List<EducationDocumentDto>> GetEducationDocumentsAsync();
    public Task<EducationDocumentDto> GetEducationDocumentByIdAsync(int id);
}