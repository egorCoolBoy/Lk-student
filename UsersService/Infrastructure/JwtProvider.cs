using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Contracts;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using UsersService.Application.JWT;
using UsersService.Domain.Entities;

namespace UsersService.Infrastructure;

public class JwtProvider : IJwtProvider
{
    private readonly JwtOptions _option;

    public JwtProvider(IOptions<JwtOptions> option)
    {
        _option = option.Value;
    }

    public string GenerateAccessToken(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role.ToString())
        };

        var secret = _option.Secret;
        var issuer = _option.Issuer;
        var audience = _option.Audience;
        var lifetime = _option.AccessTokenLifetimeMinutes;

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(secret)
        );

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(lifetime),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string GenerateRefreshToken()
    {
        return Convert.ToBase64String(Guid.NewGuid().ToByteArray());
    }
}