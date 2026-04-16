using System.Net.Http.Headers;
using System.Text;
using DirectoryService.Application.DTO;
using DirectoryService.Application.Interface;
using DirectoryService.Domain.Entities;

namespace DirectoryService.Infrastructure.Implementations;

public class KreosoftApi : IKreosoftApi
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _config;
    public KreosoftApi(HttpClient httpClient , IConfiguration config)
    {
        _config = config;
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(_config.GetValue<string>("ExternalApi:BaseUrl"));
        
        var password = _config.GetValue<string>("ExternalApi:Password");
        var login = _config.GetValue<string>("ExternalApi:Login");
        
        var credentials = $"{login}:{password}";
        var base64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(credentials));

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", base64);
    }

    public async Task<ProgramsResponseDto> GetPrograms()
    {
        var response = await _httpClient.GetAsync("programs?page=1&size=365");
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