using System.Net.Http.Headers;
using WebApplication1.Models;
using WebApplication1.Services.Dtos;

namespace WebApplication1.Services;

public class DirectoriesApi : IDirectoriesApi
{
    private readonly HttpClient _httpClient;

    public DirectoriesApi(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<ImportedDirectroriesStatistic> GetImportedData(string accessToken)
    {
        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", accessToken);
        var response = await _httpClient.GetAsync("api/directories/data");
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<ImportedDirectroriesStatistic>();
        return result;
    }

    public async Task<ImportedDirectroriesStatistic> Import(string accessToken)
    {
        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", accessToken);
        var response = await _httpClient.PostAsync("api/directories/import",null);
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<ImportedDirectroriesStatistic>();
        return result;
    }
}