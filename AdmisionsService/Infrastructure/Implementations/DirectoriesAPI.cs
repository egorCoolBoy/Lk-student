using AdmisionsService.Application.Dtos;
using AdmisionsService.Application.Interfaces;

namespace AdmisionsService.Infrastructure.Implementations;

public class DirectoriesAPI : IDirectoriesAPI
{
    private readonly HttpClient _httpClient;

    public DirectoriesAPI(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<ProgramDto> GetProgramByIdAsync(Guid id)
    {
        var response = await _httpClient.GetAsync($"/api/programs/{id}");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<ProgramDto>();
    }
    
    
}