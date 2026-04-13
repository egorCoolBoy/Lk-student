using UsersService.Domain;

namespace UsersService.Application.Users.UsersCommands;

public class RegisterCommand
{
    public string Email { get; set; }
    public string Password { get; set; }
    public Role Role { get; set; }
}