using BCrypt.Net;
using UsersService.Application.Hash;

namespace UsersService.Infrastructure.Hash;

public class PasswordHash : IPasswordHash
{
    public string Hash(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    public bool Verify(string password, string hash)
    {
        return BCrypt.Net.BCrypt.Verify(password, hash);
    }
}