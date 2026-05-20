using AdmisionsService.Application.Dtos;
using AdmisionsService.Application.Dtos.Admisions;
using AdmisionsService.Application.Interfaces;
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
    
    [HttpPost]
    public async Task<ActionResult<GetAdmissionDto>> Create(CreateAdmisisonDto dto)
    {
        var result = await _service.CreateAdmissison(dto);
        return Ok(result);
    }
    
    [HttpDelete]
    public async Task<IActionResult> Delete([FromBody] DeleteAdmission dto)
    {
        await _service.DeleteAdmissison(dto);
        return NoContent();
    }
    
    [HttpGet]
    public async Task<ActionResult<List<GetAdmissionDto>>> Get([FromQuery] AdmissionsQueryParams p)
    {
        var result = await _service.GetAdmissions(p);
        return Ok(result);
    }
    
    [HttpGet("by-ids")]
    public async Task<ActionResult<GetAdmissionDto>> GetByIds([FromQuery] GetAdmissionByIds dto)
    {
        var result = await _service.GetAdmissionByIds(dto);
        return Ok(result);
    }
    
    [HttpPut("priority")]
    public async Task<ActionResult<GetAdmissionDto>> UpdatePriority(UpdatePriorityDto dto)
    {
        var result = await _service.UpdatePriority(dto);
        return Ok(result);
    }
    
    [HttpPut("status")]
    public async Task<ActionResult<GetAdmissionDto>> UpdateStatus(UpdateStatusDto dto)
    {
        var result = await _service.UpdateStatus(dto);
        return Ok(result);
    }
    
    [HttpPut("take")]
    public async Task<ActionResult<GetAdmissionDto>> Take(ManagerAdmission dto)
    {
        var result = await _service.TakeAdmission(dto);
        return Ok(result);
    }
    
    [HttpPut("untake")]
    public async Task<ActionResult<GetAdmissionDto>> UnTake(ManagerAdmission dto)
    {
        var result = await _service.UnTakeAdmission(dto);
        return Ok(result);
    }
}