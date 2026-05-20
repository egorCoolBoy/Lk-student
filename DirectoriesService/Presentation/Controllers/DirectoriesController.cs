using Microsoft.AspNetCore.Mvc;
using DirectoryService.Application.Interface;
using Microsoft.AspNetCore.Authorization;

namespace DirectoryService.Presentation.Controllers;

[ApiController]
[Route("api/directories")]
public class DirectoriesController : ControllerBase
{
    private readonly IDirectoriesService _directoriesService;

    public DirectoriesController(IDirectoriesService directoriesService)
    {
        _directoriesService = directoriesService;
    }
    //[Authorize(Roles = "Admin")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportDirectory()
    {
        var result = await _directoriesService.ImportDirectoriesAsync();
        return Ok(result);
    }
    //[Authorize(Roles = "Admin")]
    [HttpGet("data")]
    public async Task<IActionResult> GetImportData()
    {
        var result = await _directoriesService.GetImportedDirectoriesStatistic();
        return Ok(result);
    }
}