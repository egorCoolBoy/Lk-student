using AdmisionsService.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AdmisionsService.Presentation.Controllers;
[ApiController]
[Route("api/[controller]")]
public class TestController : ControllerBase
{
    private readonly IDirectoriesAPI _directoriesAPI;
    private readonly IDocumentAPI _documentAPI;
    private readonly IUsersServiceApi _usersService;

    public TestController(IDirectoriesAPI directoriesAPI, IDocumentAPI documentAPI, IUsersServiceApi usersService)
    {
        _directoriesAPI = directoriesAPI;
        _documentAPI = documentAPI;
        _usersService = usersService;
    }

    [HttpGet]
    public async Task<IActionResult> Get(Guid id)
    {
        return Ok( await _directoriesAPI.GetProgramByIdAsync(id));
    }
    [HttpGet("docks")]
    public async Task<IActionResult> GetDocks(Guid id)
    {
        return Ok(await _documentAPI.GetDocxAsync(id));
    }

    [HttpGet("manager")]
    public async Task<IActionResult> GetManager(Guid id)
    {
        return Ok(await _usersService.GetManagerAsync(id));
    }
}