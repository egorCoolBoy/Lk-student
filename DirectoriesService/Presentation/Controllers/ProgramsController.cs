using DirectoryService.Application.Interface;
using DirectoryService.Presentation.Dto;
using Microsoft.AspNetCore.Authorization;
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
    //[Authorize]
    [HttpGet]
    public async Task<IActionResult> GetPrograms([FromQuery]ProgramsQueryDto query)
    {
        var programs = await _programsService.GetProgramsAsync(query);
        return Ok(programs);
    }

    //[Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetProgramById(Guid id)
    {
        var program = await _programsService.GetProgramByIdAsync(id);
        return Ok(program);
    }
}