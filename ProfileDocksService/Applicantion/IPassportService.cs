using ProfileDocksService.Applicantion.Dtos;

namespace ProfileDocksService.Applicantion;

public interface IPassportService
{
    Task<List<GetPassportDto>> GetPassports(Guid userId);
    Task<GetPassportDto> GetPassport(Guid passportId);
    Task<GetPassportDto> CreatePassport(Guid userId, CreatePassportDto dto);
    Task<GetPassportDto> UpdatePassport(Guid passportId, UpdatePassportDto dto);
    Task DeletePassport(Guid passportId);
}