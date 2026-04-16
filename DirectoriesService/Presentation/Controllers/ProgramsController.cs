using DirectoryService.Application.Interface;
using Microsoft.AspNetCore.Mvc;

namespace DirectoryService.Presentation.Controllers;

[ApiController]
[Route("api/programs")]
public class ProgramsController : ControllerBase
{
    private readonly IProgramsService _programsService;

    public ProgramsController(IProgramsService programsService)
    {
        _programsService = programsService;
    }

    [HttpGet]
    public async Task<IActionResult> GetPrograms([FromQuery] int page = 1, [FromQuery] int size = 10)
    {
        var programs = await _programsService.GetProgramsAsync(page, size);
        return Ok(programs);
    }


    [HttpGet("{id}")]
    public async Task<IActionResult> GetProgramById(Guid id)
    {
        var program = await _programsService.GetProgramByIdAsync(id);
        return Ok(program);
    }
}