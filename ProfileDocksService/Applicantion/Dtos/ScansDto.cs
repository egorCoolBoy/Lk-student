namespace ProfileDocksService.Applicantion;

public class PassportScanDto 
{
    public Guid Id { get; set; }
    public string OriginalFilename { get; set; } = null!;
    public string? MimeType { get; set; }
    public long FileSize { get; set; }
    public DateTime UploadedAt { get; set; }
    public Guid PassportDocumentId { get; set; }
}

public class EducationScanDto
{
    public Guid Id { get; set; }
    public string OriginalFilename { get; set; } = null!;
    public string? MimeType { get; set; }
    public long FileSize { get; set; }
    public DateTime UploadedAt { get; set; }
    public Guid EducationDocumentId { get; set; }
}

public class DownloadedFileDto
{
    public Stream Content { get; init; } = null!;
    public string ContentType { get; init; } 
    public string FileName { get; init; } = null!;
}

public class UploadScanRequest
{
    public IFormFile Scan { get; set; } = default!;
}