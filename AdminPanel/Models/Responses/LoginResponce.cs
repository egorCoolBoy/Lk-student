using Contracts;

namespace WebApplication1.Services.Dtos;

public class LoginResponse
{
    public Guid UserId { get; set; }
    public string Email{ get; set; }
    public Role Role { get; set; }
    public string AccessToken { get; set; }
}