using WebApplication1.Models;
using WebApplication1.Services.Dtos;

namespace WebApplication1.Services;

public interface IUsersServiceApi
{
    public Task<string> Login(LoginRequest loginRequest);
}