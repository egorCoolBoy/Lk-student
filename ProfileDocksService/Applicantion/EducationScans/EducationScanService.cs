using Microsoft.EntityFrameworkCore;
using ProfileDocksService.Applicantion.EducationDocks;
using ProfileDocksService.Applicantion.FileStorage;
using ProfileDocksService.Domain.Entities;
using ProfileDocksService.Infrastructure.AppDbContext;

namespace ProfileDocksService.Applicantion.EducationScans;

public class EducationScanService : IEducationScanService
{
    private readonly AppDbContext _db;
    private readonly IFileStorage _fileStorage;

    public EducationScanService(AppDbContext db, IFileStorage fileStorage)
    {
        _db = db;
        _fileStorage = fileStorage;
    }

    public async Task<List<EducationScanDto>> GetScans(Guid educationId)
    {
        var education = await _db.EducationDocuments
            .Include(e => e.Scans)
            .FirstOrDefaultAsync(e => e.Id == educationId);

        if (education == null)
            throw new KeyNotFoundException($"Education document with id {educationId} not found");

        return education.Scans
            .OrderByDescending(x => x.UploadedAt)
            .Select(ToDto)
            .ToList();
    }

    public async Task<EducationScanDto> AddScan(
        Guid educationId,
        Stream fileStream,
        string fileName,
        string contentType,
        long fileSize)
    {
        var education = await _db.EducationDocuments
            .Include(e => e.Scans)
            .FirstOrDefaultAsync(e => e.Id == educationId);
        if (education == null)
            throw new KeyNotFoundException($"Education document with id {educationId} not found");

        var storageKey = await _fileStorage.UploadAsync(fileStream, fileName, contentType);

        var scan = education.AddScan(storageKey, fileName, contentType, fileSize);
        _db.EducationScans.Add(scan);
        try
        {
            await _db.SaveChangesAsync();
        }
        catch
        {
            await _fileStorage.DeleteAsync(storageKey);
            throw;
        }

        return ToDto(scan);
    }

    public async Task<DownloadedFileDto> DownloadScan(Guid educationId, Guid scanId)
    {
        var education = await _db.EducationDocuments
            .Include(e => e.Scans)
            .FirstOrDefaultAsync(e => e.Id == educationId);

        if (education == null)
            throw new KeyNotFoundException($"Education document with id {educationId} not found");

        var scan = education.GetScan(scanId);

        if (scan == null)
            throw new KeyNotFoundException($"Education scan with id {scanId} not found");

        var content = await _fileStorage.DownloadAsync(scan.StorageKey);

        return new DownloadedFileDto
        {
            Content = content,
            ContentType = string.IsNullOrWhiteSpace(scan.MimeType) ? "application/octet-stream" : scan.MimeType,
            FileName = scan.OriginalFilename
        };
    }

    public async Task DeleteScan(Guid educationId, Guid scanId)
    {
        var education = await _db.EducationDocuments
            .Include(e => e.Scans)
            .FirstOrDefaultAsync(e => e.Id == educationId);

        if (education == null)
            throw new KeyNotFoundException($"Education document with id {educationId} not found");

        var scan = education.GetScan(scanId);

        if (scan == null)
            throw new KeyNotFoundException($"Education scan with id {scanId} not found");

        education.RemoveScan(scanId);
        await _db.SaveChangesAsync();

        await _fileStorage.DeleteAsync(scan.StorageKey);
    }

    private static EducationScanDto ToDto(EducationScan scan)
    {
        return new EducationScanDto
        {
            Id = scan.Id,
            EducationDocumentId = scan.EducationDocumentId,
            OriginalFilename = scan.OriginalFilename,
            MimeType = scan.MimeType,
            FileSize = scan.FileSize,
            UploadedAt = scan.UploadedAt
        };
    }
}