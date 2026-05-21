using System.Net.Http.Headers;
using System.Text.Json;
using WebApplication1.Models;
using WebApplication1.Services.Dtos;


namespace WebApplication1.Services;

public class UsersServiceApi : IUsersServiceApi
{
    private readonly HttpClient _httpClient;
    public UsersServiceApi(HttpClient client)
    {
        _httpClient = client;
    }
    
    public async Task<string> Login(LoginRequest loginRequest)
    {
        var response = await _httpClient.PostAsJsonAsync(
            "api/auth/login",
            loginRequest);
        response.EnsureSuccessStatusCode();

        var result = await response.Content
            .ReadFromJsonAsync<LoginResponse>();

        return result.AccessToken;
    }

    public async Task<Guid> RegisterManager(RegisterRequest request, string accessToken)
    {
        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", accessToken);
        var response = await _httpClient.PostAsJsonAsync("api/auth/register/manager", request);

        var body = await response.Content.ReadAsStringAsync();
        
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(body);
        }
        
        var userDto = await response.Content.ReadFromJsonAsync<UserDto>();
        return userDto.UserId;
    }
}