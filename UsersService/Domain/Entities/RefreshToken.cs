namespace UsersService.Domain.Entities;

public class RefreshToken
{
    public Guid Id { get; private set; }

    public string Token { get; private set; }

    public Guid UserId { get; private set; }

    public DateTime ExpiresAt { get; private set; }

    public bool IsRevoked { get; private set; }

    public DateTime CreatedAt { get; private set; }

    private RefreshToken() { }

    public RefreshToken(string token, Guid userId, DateTime expiresAt)
    {
        Id = Guid.NewGuid();
        Token = token;
        UserId = userId;
        ExpiresAt = expiresAt;
        CreatedAt = DateTime.UtcNow;
        IsRevoked = false;
    }

    public void Revoke()
    {
        IsRevoked = true;
    }

    public bool IsExpired()
    {
        return DateTime.UtcNow > ExpiresAt;
    }
}