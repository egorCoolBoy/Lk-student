using MassTransit;
using Contracts;
using ProfileDocksService.Applicantion;

namespace ProfileDocksService.Infrastructure.Consumers;

public class ApplicantCreatedConsumer : IConsumer<ApplicantCreated>
{
    private readonly IProfileService _profileService;

    public ApplicantCreatedConsumer(IProfileService profileService)
    {
        _profileService = profileService;
    }

    public async Task Consume(ConsumeContext<ApplicantCreated> context)
    {
       await _profileService.CreateProfile(context.Message.Id);
    }
}