using UsersService.Domain;
using Contracts;
namespace UsersService.Presentation.DTO.Login;

public class LoginResponce
{
    public Guid UserId { get; set; }
    public string Email{ get; set; }
    public Role Role { get; set; }
    public string AccessToken { get; set; }
}