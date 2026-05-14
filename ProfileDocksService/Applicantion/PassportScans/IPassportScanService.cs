using ProfileDocksService.Applicantion.Dtos;

namespace ProfileDocksService.Applicantion.PassportDocks;

public interface IPassportScanService
{
    Task<List<PassportScanDto>> GetScans(Guid passportId);
    Task<PassportScanDto> AddScan(Guid passportId, Stream fileStream, string fileName, string contentType, long fileSize);
    Task<DownloadedFileDto> DownloadScan(Guid passportId, Guid scanId);
    Task DeleteScan(Guid passportId, Guid scanId);
}