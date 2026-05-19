using UsersService.Domain;
using Contracts;
namespace UsersService.Application.Users.UsersResults;

public class GetMeResult
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public Role Role { get; set; }
    public string? FullName { get; set; }
}