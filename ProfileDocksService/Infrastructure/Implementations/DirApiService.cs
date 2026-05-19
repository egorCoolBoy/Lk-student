using ProfileDocksService.Applicantion.Dtos;
using ProfileDocksService.Applicantion.Interface;

namespace ProfileDocksService.Presentation.Implementations;

public class DirectoriesAPI : IDirectoriesAPI
{
    private readonly HttpClient _httpClient;

    public DirectoriesAPI(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<EducationLevelDto> GetEducationLevel(int id)
    {
        var response = await _httpClient.GetAsync($"/api/education-levels/{id}");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<EducationLevelDto>();
    }
    public async Task<EducationDocumentTypesDto> GetEducationDocumentTypeByIdAsync(Guid id)
    {
        var response = await _httpClient.GetAsync($"/api/education-documents/{id}");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<EducationDocumentTypesDto>();
    }


}