using DirectoryService.Application.DTO;

namespace DirectoryService.Application.Interface;

public interface IKreosoftApi
{
    public Task<ProgramsResponseDto> GetPrograms();
    public Task<List<FacultyDto>> GetFaculties();
    public Task<List<EducationDocumentDto>> GetEducationDocuments();
    public Task<List<EducationLevelDto>> GetEducationLevels();

}