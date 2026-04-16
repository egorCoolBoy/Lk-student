using DirectoryService.Application.Interface;
using Microsoft.AspNetCore.Mvc;

namespace DirectoryService.Presentation.Controllers;

[ApiController]
[Route("api/education-documents")]
public class EducationDocumentController : ControllerBase
{
    private readonly IEducationDocumentService _educationDocumentService;

    public EducationDocumentController(IEducationDocumentService educationDocumentService)
    {
        _educationDocumentService = educationDocumentService;
    }

    [HttpGet]
    public async Task<IActionResult> GetEducationDocuments()
    {
        var documents = await _educationDocumentService.GetEducationDocumentsAsync();
        return Ok(documents);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetEducationDocumentById(Guid id)
    {
        var document = await _educationDocumentService.GetEducationDocumentByIdAsync(id);
        return Ok(document);
    }
}