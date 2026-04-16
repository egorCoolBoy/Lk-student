using DirectoryService.Domain.Entities;

namespace DirectoryService.Application.Interface;

public interface IDirectoriesService
{
    Task<ImportedDirectroriesStatistic> ImportDirectoriesAsync();
}