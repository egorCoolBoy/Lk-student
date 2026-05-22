using System.Text.Json.Serialization;
using UsersService.Domain;
using Contracts;
namespace UsersService.Presentation.DTO;

public class RegisterRequest
{
    public string Email { get; set; }
    public string Password { get; set; }
    public Role Role { get; set; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? FullName { get; set; }
}