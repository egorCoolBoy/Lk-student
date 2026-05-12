using ProfileDocksService.Applicantion;
using ProfileDocksService.Applicantion.Dtos;

namespace ProfileDocksService.Applicantion;

public interface IEducationService
{
    Task<List<GetEducationDto>> GetEducations(Guid userId);
    Task<GetEducationDto> GetEducation(Guid educationId);
    Task<GetEducationDto> CreateEducation(Guid userId, CreateEducationDto dto);
    Task<GetEducationDto> UpdateEducation(Guid educationId, UpdateEducationDto dto);
    Task DeleteEducation(Guid educationId);
}