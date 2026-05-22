using System.Security.Claims;
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
    [Authorize(Roles = "Manager,HeadManager,Admin")]
    [HttpPost]
    public async Task<IActionResult> PostManagerFaculty(ManagerFacultyDto managerFaculty)
    {
        var id = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        var role = User.FindFirst(ClaimTypes.Role)?.Value;
        if (id != managerFaculty.ManagerId && role == "Manager")
            return Forbid();
        
        await _mfService.CreateManagerFaculty(managerFaculty);
        return Ok();
    }
    [Authorize(Roles = "Manager,HeadManager,Admin")]
    [HttpDelete]
    public async Task<IActionResult> DeleteManagerFaculty(ManagerFacultyDto managerFaculty)
    {
        var id = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        var role = User.FindFirst(ClaimTypes.Role)?.Value;
        if (id != managerFaculty.ManagerId && role == "Manager")
            return Forbid();
        
        await _mfService.DeleteManagerFaculty(managerFaculty);
        return NoContent();
    }
}