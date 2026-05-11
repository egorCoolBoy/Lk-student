using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProfileDocksService.Applicantion;
using ProfileDocksService.Applicantion.Dtos;

namespace ProfileDocksService.Presentation.Controllers;

[ApiController]
[Route("api/")]
public class PassportController : ControllerBase
{
    private readonly IPassportService _passportService;

    public PassportController(IPassportService passportService)
    {
        _passportService = passportService;
    }

    //[Authorize(Policy = "CanView")]
    [HttpGet("passports/user/{userId}")]
    public async Task<IActionResult> GetPassports(Guid userId)
    {
        var passports = await _passportService.GetPassports(userId);
        return Ok(passports);
    }

    //[Authorize(Policy = "CanView")]
    [HttpGet("passports/{passportId}")]
    public async Task<IActionResult> GetPassport(Guid passportId)
    {
        var passport = await _passportService.GetPassport(passportId);
        return Ok(passport);
    }

    //[Authorize]
    [HttpPost("passports/{userId}")]
    public async Task<IActionResult> CreatePassport(Guid userId, [FromBody] CreatePassportDto dto)
    {
        var passport = await _passportService.CreatePassport(userId, dto);
        return CreatedAtAction(nameof(GetPassport), new { passportId = passport.Id }, passport);
    }

    //[Authorize]
    [HttpPut("passports/{passportId}")]
    public async Task<IActionResult> UpdatePassport(Guid passportId, [FromBody] UpdatePassportDto dto)
    {
        var passport = await _passportService.UpdatePassport(passportId, dto);
        return Ok(passport);
    }

    //[Authorize]
    [HttpDelete("passports/{passportId}")]
    public async Task<IActionResult> DeletePassport(Guid passportId)
    {
        await _passportService.DeletePassport(passportId);
        return NoContent();
    }
}