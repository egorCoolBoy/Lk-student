using Microsoft.AspNetCore.Identity.Data;
using WebApplication1.Services.Dtos;
using LoginRequest = WebApplication1.Models.LoginRequest;
using RegisterRequest = WebApplication1.Models.RegisterRequest;

namespace WebApplication1.Services;

public interface IUsersServiceApi
{
    public Task<string> Login(LoginRequest loginRequest);
    public Task<Guid> RegisterManager(RegisterRequest request, string accessToken);
}