using AdmisionsService.Application.Dtos;
using AdmisionsService.Application.ManagerFacultyService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AdmisionsService.Presentation.Controllers;
[ApiController]
[Route("ManagerFaculty")]
public class ManagerFacultyController : ControllerBase
{
    private readonly IManagerFacultyService _mfService;

    public ManagerFacultyController(IManagerFacultyService mfService)
    {
        _mfService = mfService;
    }
    [Authorize(Roles = "Manager")]
    [HttpPost]
    public async Task<IActionResult> PostManagerFaculty(ManagerFacultyDto managerFaculty)
    {
        await _mfService.CreateManagerFaculty(managerFaculty);
        return Ok();
    }
    [Authorize(Roles = "Manager")]
    [HttpDelete]
    public async Task<IActionResult> DeleteManagerFaculty(ManagerFacultyDto managerFaculty)
    {
        await _mfService.DeleteManagerFaculty(managerFaculty);
        return NoContent();
    }
}