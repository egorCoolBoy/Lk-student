using DirectoryService.Application.Interface;
using Microsoft.AspNetCore.Mvc;

namespace DirectoryService.Presentation.Controllers;

[ApiController]
[Route("api/education-levels")]
public class EducationLevelController : ControllerBase
{
    private readonly IEducationLevelService _educationLevelService;

    public EducationLevelController(IEducationLevelService educationLevelService)
    {
        _educationLevelService = educationLevelService;
    }
    //[Authorize]
    [HttpGet]
    public async Task<IActionResult> GetEducationLevels()
    {
        var levels = await _educationLevelService.GetEducationLevelsAsync();
        return Ok(levels);
    }   
    //[Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetEducationLevelById(int id)
    {
        var level = await _educationLevelService.GetEducationLevelByIdAsync(id);
        return Ok(level);
    }
}