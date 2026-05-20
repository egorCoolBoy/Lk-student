using WebApplication1.Models;
using WebApplication1.Services.Dtos;

namespace WebApplication1.Services;

public class UsersServiceApi : IUsersServiceApi
{
    private readonly HttpClient _client;
    public UsersServiceApi(HttpClient client)
    {
        _client = client;
    }
    
    public async Task<string> Login(LoginRequest loginRequest)
    {
        var response = await _client.PostAsJsonAsync(
            "api/auth/login",
            loginRequest);

        response.EnsureSuccessStatusCode();

        var result = await response.Content
            .ReadFromJsonAsync<LoginResponse>();

        return result.AccessToken;
    }
}