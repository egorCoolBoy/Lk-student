using UsersService.Domain;
using Contracts;
namespace UsersService.Application.Users.UsersCommands;

public class RegisterManagerCommand
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string FullName { get; set; }
    public Role Role{ get; set; } 
}