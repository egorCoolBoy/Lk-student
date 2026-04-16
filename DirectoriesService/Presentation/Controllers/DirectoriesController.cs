using Microsoft.AspNetCore.Mvc;
using DirectoryService.Application.Interface;

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

    [HttpPost("import")]
    public async Task<IActionResult> ImportDirectory()
    {
        var result = await _directoriesService.ImportDirectoriesAsync();
        return Ok(result);
    }
}