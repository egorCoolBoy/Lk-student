using System.Net.Http.Headers;
using System.Text;
using DirectoryService.Application.DTO;
using DirectoryService.Application.Interface;
using DirectoryService.Domain.Entities;

namespace DirectoryService.Infrastructure.Implementations;

public class KreosoftApi : IKreosoftApi
{
    private readonly HttpClient _httpClient;

    public KreosoftApi(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<ProgramsResponseDto> GetPrograms()
    {
        var response = await _httpClient.GetAsync("programs?page=1&size=1000");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<ProgramsResponseDto>();
    }

    public async Task<List<FacultyDto>> GetFaculties()
    {
        var response = await _httpClient.GetAsync("faculties");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<List<FacultyDto>>();
    }

    public async Task<List<EducationDocumentDto>> GetEducationDocuments()
    {
        var response = await _httpClient.GetAsync("document_types");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<List<EducationDocumentDto>>();
    }

    public async Task<List<EducationLevelDto>> GetEducationLevels()
    {
        var response = await _httpClient.GetAsync("education_levels");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<List<EducationLevelDto>>();
    }
}