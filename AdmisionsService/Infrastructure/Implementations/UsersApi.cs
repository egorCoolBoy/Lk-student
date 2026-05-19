using AdmisionsService.Application.Dtos;
using AdmisionsService.Application.Interfaces;

namespace AdmisionsService.Infrastructure.Implementations;

public class UsersServiceApi : IUsersServiceApi
{
    private readonly HttpClient _client;

    public UsersServiceApi(HttpClient client)
    {
        _client = client;
    }

    public async Task<GetManager> GetManagerAsync(Guid id)
    {
        var response = await _client.GetAsync($"/api/auth/manager/{id}");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<GetManager>();
    }
}