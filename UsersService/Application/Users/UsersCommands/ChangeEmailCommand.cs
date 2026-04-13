namespace UsersService.Application.Users.UsersCommands;

public class ChangeEmailCommand
{
    public Guid UserId { get; set; } 
    public string NewEmail { get; set; }
    public string Password { get; set; }
}