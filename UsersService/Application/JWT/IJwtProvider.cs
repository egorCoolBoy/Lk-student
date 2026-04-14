using UsersService.Domain.Entities;

namespace UsersService.Application.JWT;

public interface IJwtProvider
{
    string GenerateAccessToken(User user);
    string GenerateRefreshToken();
}