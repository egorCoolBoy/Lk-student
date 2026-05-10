using ProfileDocksService.Domain.Enums;

namespace ProfileDocksService.Domain.Entities;

public class PassportScan
{
    public Guid Id { get; set; }

    public Guid PassportDocumentId { get; set; }

    public string StorageKey { get; set; } = null!;

    public string OriginalFilename { get; set; } = null!;

    public string? MimeType { get; set; } = null!;

    public long FileSize { get; set; }

    public DateTime UploadedAt { get; set; }

    public PassportDocument PassportDocument { get; set; } = null!;
}

public class EducationScan
{
    public Guid Id { get; set; }

    public Guid EducationDocumentId { get; set; }

    public string StorageKey { get; set; } = null!;

    public string OriginalFilename { get; set; } = null!;

    public string? MimeType { get; set; } = null!;

    public long FileSize { get; set; }

    public DateTime UploadedAt { get; set; }

    public EducationDocument EducationDocument { get; set; } = null!;
}