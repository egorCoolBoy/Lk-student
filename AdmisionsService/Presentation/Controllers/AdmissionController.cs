using System.Security.Claims;
using AdmisionsService.Application.Dtos;
using AdmisionsService.Application.Dtos.Admisions;
using AdmisionsService.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AdmisionsService.API.Controllers;

[ApiController]
[Route("api/admissions")]
public class AdmissionController : ControllerBase
{
    private readonly IAdmissionService _service;

    public AdmissionController(IAdmissionService service)
    {
        _service = service;
    }
    [Authorize(Roles = "Applicant,Admin")]
    [HttpPost]
    public async Task<ActionResult<GetAdmissionDto>> Create(CreateAdmisisonDto dto)
    {
        var result = await _service.CreateAdmissison(dto);
        return Ok(result);
    }
    [Authorize]
    [HttpDelete]
    public async Task<IActionResult> Delete([FromBody] DeleteAdmission dto)
    {
        var id = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        var role = User.FindFirst(ClaimTypes.Role)?.Value;
        if (id != dto.UserdId && role == "Applicant")
            return Forbid();
        
        await _service.DeleteAdmissison(dto);
        return NoContent();
    }
    [Authorize(Roles = "Manager,HeadManager,Admin")]
    [HttpGet]
    public async Task<ActionResult<List<GetAdmissionDto>>> Get([FromQuery] AdmissionsQueryParams p)
    {
        var result = await _service.GetAdmissions(p);
        return Ok(result);
    }
    [Authorize]
    [HttpGet("by-ids")]
    public async Task<ActionResult<GetAdmissionDto>> GetByIds([FromQuery] GetAdmissionByIds dto)
    {
        var id = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        var role = User.FindFirst(ClaimTypes.Role)?.Value;
        if (id != dto.ApplicantId && role == "Applicant")
            return Forbid();
        
        var result = await _service.GetAdmissionByIds(dto);
        return Ok(result);
    }
    [Authorize]
    [HttpPut("priority")]
    public async Task<ActionResult<GetAdmissionDto>> UpdatePriority(UpdatePriorityDto dto)
    {
        var id = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        var role = User.FindFirst(ClaimTypes.Role)?.Value;
        if (id != dto.ApplicantId && role == "Applicant")
            return Forbid();
        
        var result = await _service.UpdatePriority(dto);
        return Ok(result);
    }
    [Authorize(Roles = "Manager, HeadManager,Admin")]
    [HttpPut("status")]
    public async Task<ActionResult<GetAdmissionDto>> UpdateStatus(UpdateStatusDto dto)
    {
        var result = await _service.UpdateStatus(dto);
        return Ok(result);
    }
    [Authorize(Roles = "Manager, HeadManager,Admin")]
    [HttpPut("take")]
    public async Task<ActionResult<GetAdmissionDto>> Take(ManagerAdmission dto)
    {
        var id = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        var role = User.FindFirst(ClaimTypes.Role)?.Value;
        if (id != dto.ManagerId && role == "Manager")
            return Forbid();
        var result = await _service.TakeAdmission(dto);
        return Ok(result);
    }
    [Authorize(Roles = "Manager, HeadManager,Admin")]
    [HttpPut("unTake")]
    public async Task<ActionResult<GetAdmissionDto>> UnTake(ManagerAdmission dto)
    {
        var id = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        var role = User.FindFirst(ClaimTypes.Role)?.Value;
        if (id != dto.ManagerId && role == "Manager")
            return Forbid();
        var result = await _service.UnTakeAdmission(dto);
        return Ok(result);
    }

    [Authorize]
    [HttpGet("{applicantId}")]
    public async Task<ActionResult<GetAdmissionDto>> GetById(Guid applicantId)
    {
        var id = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        var role = User.FindFirst(ClaimTypes.Role)?.Value;
        if (id != applicantId && role == "Applicant")
            return Forbid();
        
        var res = await _service.GetAdmissionByApplicantId(applicantId);
        return Ok(res);
    }
}