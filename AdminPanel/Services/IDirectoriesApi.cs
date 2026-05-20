using WebApplication1.Models;
using WebApplication1.Services.Dtos;

namespace WebApplication1.Services;

public interface IDirectoriesApi
{
    public Task<ImportedDirectroriesStatistic> GetImportedData();
    public Task<ImportedDirectroriesStatistic> Import();
}