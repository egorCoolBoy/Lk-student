using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProfileDocksService.Applicantion;
using ProfileDocksService.Applicantion.Dtos;

namespace ProfileDocksService.Presentation.Controllers;

[ApiController]
[Route("api/")]
public class ProfilesControllers : ControllerBase
{
    private readonly IProfileService _profileService;

    public ProfilesControllers(IProfileService profileService)
    {
        _profileService = profileService;
    }
    [HttpGet("profiles/{userId}")]
    public async Task<IActionResult> Getrofile(Guid userId)
    {
        var profile =  await _profileService.GetProfile(userId);
        return Ok(profile);
    }

    [Authorize]
    [HttpPut("profiles/{userId}")]
    public async Task<IActionResult> UpdateProfile(UpdateProfileDto request)
    {
        var id = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        var role = User.FindFirst(ClaimTypes.Role)?.Value;
        if (id != request.UserId && role == "Applicant")
            return Forbid();
        var newProfile = await _profileService.UpdateProfile(request);
        return Ok(newProfile);
    }
}