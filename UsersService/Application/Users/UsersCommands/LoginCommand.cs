namespace UsersService.Application.Users.UsersCommands;

public class LoginCommand
{
    public Guid UserId { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}