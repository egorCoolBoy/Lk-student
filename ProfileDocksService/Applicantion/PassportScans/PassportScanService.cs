using Microsoft.EntityFrameworkCore;
using ProfileDocksService.Applicantion.FileStorage;
using ProfileDocksService.Applicantion.PassportDocks;
using ProfileDocksService.Domain.Entities;
using ProfileDocksService.Infrastructure.AppDbContext;

namespace ProfileDocksService.Applicantion.PassportScans;

public class PassportScanService : IPassportScanService
{
    private readonly AppDbContext _db;
    private readonly IFileStorage _fileStorage;

    public PassportScanService(AppDbContext db, IFileStorage fileStorage)
    {
        _db = db;
        _fileStorage = fileStorage;
    }

    public async Task<List<PassportScanDto>> GetScans(Guid passportId)
    {
        var passport = await _db.PassportDocuments
            .Include(p => p.Scans)
            .FirstOrDefaultAsync(p => p.Id == passportId);

        if (passport == null)
            throw new KeyNotFoundException($"Passport with id {passportId} not found");

        return passport.Scans
            .OrderByDescending(x => x.UploadedAt)
            .Select(ToDto)
            .ToList();
    }

    public async Task<PassportScanDto> AddScan(
        Guid passportId,
        Stream fileStream,
        string fileName,
        string contentType,
        long fileSize)
    {
        var passport = await _db.PassportDocuments
            .Include(p => p.Scans)
            .FirstOrDefaultAsync(p => p.Id == passportId);
        if (passport == null)
            throw new KeyNotFoundException($"Passport with id {passportId} not found");

        var storageKey = await _fileStorage.UploadAsync(fileStream, fileName, contentType);

        var scan = passport.AddScan(storageKey, fileName, contentType, fileSize);
        _db.PassportScans.Add(scan);

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

    public async Task<DownloadedFileDto> DownloadScan(Guid passportId, Guid scanId)
    {
        var passport = await _db.PassportDocuments
            .Include(p => p.Scans)
            .FirstOrDefaultAsync(p => p.Id == passportId);

        if (passport == null)
            throw new KeyNotFoundException($"Passport with id {passportId} not found");

        var scan = passport.GetScan(scanId);

        if (scan == null)
            throw new KeyNotFoundException($"Passport scan with id {scanId} not found");

        var content = await _fileStorage.DownloadAsync(scan.StorageKey);

        return new DownloadedFileDto
        {
            Content = content,
            ContentType = string.IsNullOrWhiteSpace(scan.MimeType) ? "application/octet-stream" : scan.MimeType,
            FileName = scan.OriginalFilename
        };
    }

    public async Task DeleteScan(Guid passportId, Guid scanId)
    {
        var passport = await _db.PassportDocuments
            .Include(p => p.Scans)
            .FirstOrDefaultAsync(p => p.Id == passportId);

        if (passport == null)
            throw new KeyNotFoundException($"Passport with id {passportId} not found");

        var scan = passport.GetScan(scanId);

        if (scan == null)
            throw new KeyNotFoundException($"Passport scan with id {scanId} not found");

        passport.RemoveScan(scanId);
        
        await _db.SaveChangesAsync();

        await _fileStorage.DeleteAsync(scan.StorageKey);
    }

    private static PassportScanDto ToDto(PassportScan scan)
    {
        return new PassportScanDto
        {
            Id = scan.Id,
            PassportDocumentId = scan.PassportDocumentId,
            OriginalFilename = scan.OriginalFilename,
            MimeType = scan.MimeType,
            FileSize = scan.FileSize,
            UploadedAt = scan.UploadedAt
        };
    }
}