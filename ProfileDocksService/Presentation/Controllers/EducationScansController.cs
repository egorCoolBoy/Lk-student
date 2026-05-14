using Microsoft.AspNetCore.Mvc;
using ProfileDocksService.Applicantion;
using ProfileDocksService.Applicantion.EducationDocks;

namespace ProfileDocksService.Presentation.Controllers;

[ApiController]
[Route("api/educations/{educationId}/scans")]
public class EducationScansController : ControllerBase
{
    private readonly IEducationScanService _educationScanService;

    public EducationScansController(IEducationScanService educationScanService)
    {
        _educationScanService = educationScanService;
    }

    [HttpGet]
    //[Authorize(Policy = "CanView")]
    public async Task<IActionResult> GetScans(Guid educationId)
    {
        var scans = await _educationScanService.GetScans(educationId);
        return Ok(scans);
    }

    [HttpPost]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> AddScan(Guid educationId, [FromForm] UploadScanRequest request)
    {
        var scan = request.Scan;
        if (scan == null || scan.Length == 0)
            throw new ArgumentException("Scan file is required");

        var result = await _educationScanService.AddScan(
            educationId,
            scan.OpenReadStream(),
            scan.FileName,
            string.IsNullOrWhiteSpace(scan.ContentType) ? "application/octet-stream" : scan.ContentType,
            scan.Length);

        return Ok(result);
    }

    [HttpGet("{scanId}/download")]
    //[Authorize(Policy = "CanView")]
    public async Task<IActionResult> DownloadScan(Guid educationId, Guid scanId)
    {
        var result = await _educationScanService.DownloadScan(educationId, scanId);
        return File(result.Content, result.ContentType, result.FileName);
    }

    [HttpDelete("{scanId}")]
    public async Task<IActionResult> DeleteScan(Guid educationId, Guid scanId)
    {
        await _educationScanService.DeleteScan(educationId, scanId);
        return NoContent();
    }
}