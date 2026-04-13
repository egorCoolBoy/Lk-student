using UsersService.Domain;

namespace UsersService.Presentation.DTO;

public class GetMeResponce
{
    public Guid UserId { get; set; }
    public string Email { get; set; }
    public Role Role { get; set; }
}