using UsersService.Domain;
using Contracts;
namespace UsersService.Application.Users.UsersResults;

public class LoginResult
{
    public Guid UserId { get; set; }
    public string Email{ get; set; }
    public Role Role { get; set; }
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
}