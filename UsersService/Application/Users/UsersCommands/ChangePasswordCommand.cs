namespace UsersService.Application.Users.UsersCommands;

public class ChangePasswordCommand
{
    public Guid UserId { get; set; } 
    public string CurrentPassword { get; set; }
    public string NewPassword { get; set; }
    public string ConfirmNewPassword { get; set; }
}