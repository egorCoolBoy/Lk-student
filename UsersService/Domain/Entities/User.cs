using Contracts;

namespace UsersService.Domain.Entities;

public class User
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public string HashedPassword { get; set; }
    public Role Role { get; set; }
    public string? FullName { get; set; }
    public List<RefreshToken> RefreshTokens { get; set; }

    public User(string email, string hashedPassword, Role role,string? fullName = null)
    {
        Id = Guid.NewGuid();
        Email = email;
        HashedPassword = hashedPassword;
        Role = role;
        FullName = fullName;
    }

    public void ChangeEmail(string email)
    {
        if (email == Email)
            throw new InvalidOperationException("Email must be different");
        Email = email; 
    }
    
    public void ChangeHashedPassword(string hashedPassword)
    {
        if (hashedPassword == HashedPassword) 
            throw new InvalidOperationException("Password must be different");
        HashedPassword = hashedPassword;
    }
}