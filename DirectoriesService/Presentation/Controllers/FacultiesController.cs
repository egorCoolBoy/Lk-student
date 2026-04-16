using DirectoryService.Application.Interface;
using Microsoft.AspNetCore.Mvc;

namespace DirectoryService.Presentation.Controllers;

[ApiController]
[Route("api/faculties")]
public class FacultiesController : ControllerBase
{
    private readonly IFacultiesService _facultiesService;

    public FacultiesController(IFacultiesService facultiesService)
    {
        _facultiesService = facultiesService;
    }

    [HttpGet]
    public async Task<IActionResult> GetFaculties()
    {
        var faculties = await _facultiesService.GetFacultiesAsync();
        return Ok(faculties);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetFacultyById(Guid id)
    {
        var faculty = await _facultiesService.GetFacultyByIdAsync(id);
        return Ok(faculty);
    }
}