using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProfileDocksService.Applicantion;
using ProfileDocksService.Applicantion.PassportDocks;

namespace ProfileDocksService.Presentation.Controllers;

[ApiController]
[Route("api/passports/{passportId:guid}/scans")]
public class PassportScansController : ControllerBase
{
    private readonly IPassportScanService _passportScanService;

    public PassportScansController(IPassportScanService passportScanService)
    {
        _passportScanService = passportScanService;
    }
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetScans(Guid passportId)
    {
        var scans = await _passportScanService.GetScans(passportId);
        return Ok(scans);
    }
    [Authorize]
    [HttpPost]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> AddScan(Guid passportId, [FromForm] UploadScanRequest request)
    {
        var scan = request.Scan;
        if (scan == null || scan.Length == 0)
            throw new ArgumentException("Scan file is required");

        var result = await _passportScanService.AddScan(
            passportId,
            scan.OpenReadStream(),
            scan.FileName,
            string.IsNullOrWhiteSpace(scan.ContentType) ? "application/octet-stream" : scan.ContentType,
            scan.Length);

        return Ok(result);
    }
    [Authorize]
    [HttpGet("{scanId:guid}/download")]
    public async Task<IActionResult> DownloadScan(Guid passportId, Guid scanId)
    {
        var result = await _passportScanService.DownloadScan(passportId, scanId);
        return File(result.Content, result.ContentType, result.FileName);
    }
    [Authorize]
    [HttpDelete("{scanId:guid}")]
    public async Task<IActionResult> DeleteScan(Guid passportId, Guid scanId)
    {
        await _passportScanService.DeleteScan(passportId, scanId);
        return NoContent();
    }
    
    
}