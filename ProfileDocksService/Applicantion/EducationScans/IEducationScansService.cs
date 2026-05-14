using ProfileDocksService.Applicantion.Dtos;

namespace ProfileDocksService.Applicantion.EducationDocks;

public interface IEducationScanService
{
    Task<List<EducationScanDto>> GetScans(Guid educationId);
    Task<EducationScanDto> AddScan(Guid educationId, Stream fileStream, string fileName, string contentType, long fileSize);
    Task<DownloadedFileDto> DownloadScan(Guid educationId, Guid scanId);
    Task DeleteScan(Guid educationId, Guid scanId);
}