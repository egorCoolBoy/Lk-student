using UsersService.Domain;

namespace UsersService.Presentation.DTO;

public class RegisterManagerRequest
{
    public string Email { get; set; }
    public string Password { get; set; }
}