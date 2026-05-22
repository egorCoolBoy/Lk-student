using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProfileDocksService.Applicantion;
using ProfileDocksService.Applicantion.Dtos;

namespace ProfileDocksService.Presentation.Controllers;

[ApiController]
[Route("api/")]
public class EducationController : ControllerBase
{
    private readonly IEducationService _educationService;

    public EducationController(IEducationService educationService)
    {
        _educationService = educationService;
    }

    //[Authorize(Policy = "CanView")]
    [HttpGet("educations/user/{userId}")]
    public async Task<IActionResult> GetEducations(Guid userId)
    {
        var educations = await _educationService.GetEducations(userId);
        return Ok(educations);
    }

    [Authorize]
    [HttpGet("educations/{educationId}")]
    public async Task<IActionResult> GetEducation(Guid educationId)
    {
        var education = await _educationService.GetEducation(educationId);
        return Ok(education);
    }

    [Authorize]
    [HttpPost("educations/{userId}")]
    public async Task<IActionResult> CreateEducation(Guid userId, [FromBody] CreateEducationDto dto)
    {
        var id = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        var role = User.FindFirst(ClaimTypes.Role)?.Value;
        if (id != userId && role == "Applicant")
            return Forbid();
        
        var education = await _educationService.CreateEducation(userId, dto);
        return CreatedAtAction(nameof(GetEducation), new { educationId = education.Id }, education);
    }

    [Authorize]
    [HttpPut("educations/{educationId}")]
    public async Task<IActionResult> UpdateEducation(Guid educationId, [FromBody] UpdateEducationDto dto)
    {
        var education = await _educationService.UpdateEducation(educationId, dto);
        return Ok(education);
    }

    [Authorize]
    [HttpDelete("educations/{educationId}")]
    public async Task<IActionResult> DeleteEducation(Guid educationId)
    {
        await _educationService.DeleteEducation(educationId);
        return NoContent();
    }
}