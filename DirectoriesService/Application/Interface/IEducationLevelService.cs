using DirectoryService.Application.DTO;
namespace DirectoryService.Application.Interface;

public interface IEducationLevelService
{
    public Task<List<EducationLevelDto>> GetEducationLevelsAsync();
    public Task<EducationLevelDto> GetEducationLevelByIdAsync(int id);
}