using ProfileDocksService.Domain.Enums;

namespace ProfileDocksService.Domain.Entities;

public class EducationDocument
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public Guid EducationTypeId { get; private set; }
    public string EducationName { get; set; }
    public string LevelName { get; private set; }
    public int LevelId { get; private set; }
    public string Specialty { get; private set; }
    public DateOnly GraduationDate { get; private set; }
    public string DiplomaNumber { get; private set; }

    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    private readonly List<EducationScan> _scans = new();
    public IReadOnlyCollection<EducationScan> Scans => _scans;

    private EducationDocument() { } 

    public EducationDocument(
        Guid userId,
        string level,
        string specialty,
        DateOnly graduationDate,
        string diplomaNumber,int levelId,Guid educationTypeId,string educationName)
    {
        Id = Guid.NewGuid();

        UserId = userId;
        EducationTypeId = educationTypeId;
        EducationName = educationName;
        LevelName = level;
        Specialty = specialty;
        GraduationDate = graduationDate;
        DiplomaNumber = diplomaNumber;
        LevelId = levelId;
        CreatedAt = DateTime.UtcNow;
    }
    
    public void UpdateDetails(
        string? specialty = null,
        DateOnly? graduationDate = null,
        string? diplomaNumber = null,
        string? levelName = null, 
        int? levelId = null, Guid? educationTypeId = null,string? educationName=null)
    {
        var changed = false;

        if (educationTypeId.HasValue && educationTypeId!=EducationTypeId)
        {
            EducationName = educationName;
            EducationTypeId = educationTypeId.Value;
            LevelName = levelName;
            LevelId = levelId.Value;
            changed = true;
        }

        if (!string.IsNullOrWhiteSpace(specialty) && specialty != Specialty)
        {
            Specialty = specialty;
            changed = true;
        }

        if (graduationDate.HasValue && graduationDate.Value != GraduationDate)
        {
            GraduationDate = graduationDate.Value;
            changed = true;
        }

        if (!string.IsNullOrWhiteSpace(diplomaNumber) && diplomaNumber != DiplomaNumber)
        {
            DiplomaNumber = diplomaNumber;
            changed = true;
        }
        if (changed)
            UpdatedAt = DateTime.UtcNow;
    }
    public EducationScan? GetScan(Guid scanId)
    {
        return _scans.FirstOrDefault(x => x.Id == scanId);
    }

    public EducationScan AddScan(
        string storageKey,
        string originalFilename,
        string? mimeType,
        long fileSize)
    {
        var scan = new EducationScan
        {
            Id = Guid.NewGuid(),
            EducationDocumentId = Id,
            StorageKey = storageKey,
            OriginalFilename = originalFilename,
            MimeType = mimeType,
            FileSize = fileSize,
            UploadedAt = DateTime.UtcNow,
            EducationDocument = this
        };

        _scans.Add(scan);

        UpdatedAt = DateTime.UtcNow;

        return scan;
    }

    public void RemoveScan(Guid scanId)
    {
        var scan = _scans.FirstOrDefault(x => x.Id == scanId);

        if (scan is null)
            throw new KeyNotFoundException("Scan not found");

        _scans.Remove(scan);

        UpdatedAt = DateTime.UtcNow;
    }
}

public class PassportDocument
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }

    public string Series { get; private set; }
    public string Number { get; private set; }
    public string IssuedBy { get; private set; }
    public DateOnly IssuedDate { get; private set; }

    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    private readonly List<PassportScan> _scans = new();
    public IReadOnlyCollection<PassportScan> Scans => _scans;

    private PassportDocument() { }

    public PassportDocument(
        Guid userId,
        string series,
        string number,
        string issuedBy,
        DateOnly issuedDate)
    {
        Id = Guid.NewGuid();

        UserId = userId;

        Series = series;
        Number = number;
        IssuedBy = issuedBy;
        IssuedDate = issuedDate;

        CreatedAt = DateTime.UtcNow;
    }

    public void UpdateDetails(
        string? series = null,
        string? number = null,
        string? issuedBy = null,
        DateOnly? issuedDate = null)
    {
        var changed = false;

        if (!string.IsNullOrWhiteSpace(series) && series != Series)
        {
            Series = series;
            changed = true;
        }

        if (!string.IsNullOrWhiteSpace(number) && number != Number)
        {
            Number = number;
            changed = true;
        }

        if (!string.IsNullOrWhiteSpace(issuedBy) && issuedBy != IssuedBy)
        {
            IssuedBy = issuedBy;
            changed = true;
        }

        if (issuedDate.HasValue && issuedDate.Value != IssuedDate)
        {
            IssuedDate = issuedDate.Value;
            changed = true;
        }

        if (changed)
            UpdatedAt = DateTime.UtcNow;
    }

    public PassportScan AddScan(
        string storageKey,
        string originalFilename,
        string? mimeType,
        long fileSize)
    {
        var scan = new PassportScan
        {
            Id = Guid.NewGuid(),
            PassportDocumentId = Id,

            StorageKey = storageKey,
            OriginalFilename = originalFilename,
            MimeType = mimeType,
            FileSize = fileSize,

            UploadedAt = DateTime.UtcNow,
            PassportDocument = this
        };

        _scans.Add(scan);

        UpdatedAt = DateTime.UtcNow;

        return scan;
    }
    public PassportScan? GetScan(Guid scanId)
    {
        return _scans.FirstOrDefault(x => x.Id == scanId);
    }

    public void RemoveScan(Guid scanId)
    {
        var scan = _scans.FirstOrDefault(x => x.Id == scanId);

        if (scan is null)
            throw new KeyNotFoundException("Scan not found");

        _scans.Remove(scan);

        UpdatedAt = DateTime.UtcNow;
    }
}