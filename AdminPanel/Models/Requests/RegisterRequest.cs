namespace WebApplication1.Models;

public class RegisterRequest
{
    public string Email { get; set; }
    public string Password { get; set; }
    public Role Role { get; set; }
    public string? FullName { get; set; }
}