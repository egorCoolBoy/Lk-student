using AdmisionsService.Application.Dtos;
using AdmisionsService.Application.Interfaces;

namespace AdmisionsService.Infrastructure.Implementations;

public class DocumentAPI : IDocumentAPI
{
    private readonly HttpClient _httpClient;
    
    public DocumentAPI(HttpClient httpClient)
    { 
        _httpClient = httpClient;
    }

    public async Task<List<EducationDocxDto>> GetDocxAsync(Guid userId)
    {
        var response = await _httpClient.GetAsync($"/api/educations/user/{userId}");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<List<EducationDocxDto>>();
    }

    public async Task<GetProfileDto> GetProfileAsync(Guid userId)
    {
        var response = await _httpClient.GetAsync($"/api/profile/{userId}");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<GetProfileDto>();
    }
}