namespace UsersService.Application.Users.UsersResults;

public class GetMeResult
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public string Role { get; set; }
}