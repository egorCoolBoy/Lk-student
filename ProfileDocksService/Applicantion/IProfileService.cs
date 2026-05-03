using ProfileDocksService.Applicantion.Dtos;

namespace ProfileDocksService.Applicantion;

public interface IProfileService
{
    public Task<GetProfileDto>  GetProfile(Guid userId);
    public Task<UpdateProfileDto> UpdateProfile(UpdateProfileDto profile);
}