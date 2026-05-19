using Contracts;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using UsersService.Application.Hash;
using UsersService.Application.JWT;
using UsersService.Application.Users.Queries;
using UsersService.Application.Users.UsersCommands;
using UsersService.Application.Users.UsersResults;
using UsersService.Domain;
using UsersService.Domain.Entities;
using UsersService.Infrastructure.AppDbContext;

namespace UsersService;

public class UserService : IUsersService
{
    private readonly AppDbContext _dbContext;
    private readonly IPasswordHash _passwordHasher;
    private readonly IJwtProvider _jwtProvider;
    private readonly IConfiguration _config;
    private readonly IPublishEndpoint _publish;

    public UserService(AppDbContext dbContext,
        IPasswordHash passwordHasher,
        IJwtProvider jwtProvider,
        IConfiguration config,
        IPublishEndpoint publish)
    {
        _dbContext = dbContext;
        _passwordHasher = passwordHasher;
        _jwtProvider = jwtProvider;
        _config = config;
        _publish = publish;
    }

    public async Task<RegisterResult> RegisterAsync(RegisterCommand command)
    {
        if (await EmailExists(command.Email))
        {
            throw new InvalidOperationException("Email is already exists");
        }

        if (command.Role == Role.Admin)
            throw new InvalidOperationException("Admin role is already registered");

        var passwordHash = _passwordHasher.Hash(command.Password);

        command.FullName = command.Role != Role.Applicant ? command.FullName : null;

        var user = new User(command.Email, passwordHash, command.Role, command.FullName);

        if (user.Role == Role.Applicant)
        {
            await _publish.Publish(new ApplicantCreated() { Id = user.Id });
        }

        await _publish.Publish(new ApplicantCreated() { Id = user.Id });
        await _dbContext.Users.AddAsync(user);
        await _dbContext.SaveChangesAsync();


        return new RegisterResult
        {
            UserId = user.Id
        };
    }

    public async Task<RegisterResult> RegisterManagerAsync(RegisterManagerCommand command)
    {
        if (await EmailExists(command.Email))
        {
            throw new InvalidOperationException("Email is already exists");
        }

        if (command.Role == Role.Admin || command.Role == Role.Applicant)
            throw new InvalidOperationException("Invalid Role");
        var passwordHash = _passwordHasher.Hash(command.Password);

        var user = new User(command.Email, passwordHash, command.Role);
        var message = new ManagerCreated()
        {
            Email = user.Email,
            Id = user.Id
        };
        await _publish.Publish(message);
        await _dbContext.Users.AddAsync(user);
        await _dbContext.SaveChangesAsync();



        return new RegisterResult
        {
            UserId = user.Id
        };
    }

    public async Task<LoginResult> LoginAsync(LoginCommand command)
    {
        var user = await _dbContext.Users
            .FirstOrDefaultAsync(x => x.Email == command.Email);

        if (user == null)
            throw new InvalidOperationException("Invalid credentials");

        if (!_passwordHasher.Verify(command.Password, user.HashedPassword))
            throw new InvalidOperationException("Invalid credentials");

        var accessToken = _jwtProvider.GenerateAccessToken(user);
        var refreshToken = _jwtProvider.GenerateRefreshToken();

        var refresh = new RefreshToken
        (
            refreshToken,
            user.Id,
            DateTime.UtcNow.AddDays(_config.GetValue<int>("Jwt:RefreshTokenLifetimeDays"))
        );

        _dbContext.RefreshTokens.Add(refresh);
        await _dbContext.SaveChangesAsync();

        return new LoginResult
        {
            UserId = user.Id,
            Email = user.Email,
            Role = user.Role,
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
    }

    public async Task LogoutAsync(LogoutCommand command)
    {
        var token = await _dbContext.RefreshTokens
            .FirstOrDefaultAsync(x => x.Token == command.RefreshToken);

        if (token == null)
            return;

        token.Revoke();

        await _dbContext.SaveChangesAsync();
    }

    public async Task<RefreshTokenResult> RefreshTokenAsync(RefreshTokenCommand command)
    {
        var storedToken = await _dbContext.RefreshTokens
            .FirstOrDefaultAsync(x => x.Token == command.RefreshToken);

        if (storedToken == null || storedToken.IsRevoked || storedToken.ExpiresAt < DateTime.UtcNow)
            throw new InvalidOperationException("Invalid refresh token");

        var user = await _dbContext.Users.FindAsync(storedToken.UserId);
        if (user == null)
            throw new InvalidOperationException("User not found");

        var newAccessToken = _jwtProvider.GenerateAccessToken(user);
        var newRefreshToken = _jwtProvider.GenerateRefreshToken();

        storedToken.Revoke();
        var refreshToken = new RefreshToken
        (
            newRefreshToken,
            user.Id,
            DateTime.UtcNow.AddDays(_config.GetValue<int>("Jwt:RefreshTokenLifetimeDays"))
        );

        _dbContext.RefreshTokens.Add(refreshToken);

        await _dbContext.SaveChangesAsync();

        return new RefreshTokenResult
        {
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken
        };
    }

    public async Task ChangePasswordAsync(ChangePasswordCommand command)
    {
        var user = await _dbContext.Users
            .FirstOrDefaultAsync(x => x.Id == command.UserId);

        if (user == null)
            throw new InvalidOperationException("User not found");

        if (!_passwordHasher.Verify(command.CurrentPassword, user.HashedPassword))
            throw new InvalidOperationException("Invalid password");

        user.ChangeHashedPassword(
            _passwordHasher.Hash(command.NewPassword)
        );

        await _dbContext.SaveChangesAsync();
    }

    public async Task ChangeEmailAsync(ChangeEmailCommand command)
    {
        var user = await _dbContext.Users
            .FirstOrDefaultAsync(x => x.Id == command.UserId);

        if (user == null)
            throw new InvalidOperationException("User not found");

        if (!_passwordHasher.Verify(command.Password, user.HashedPassword))
            throw new InvalidOperationException("Invalid password");

        if (await _dbContext.Users.AnyAsync(x => x.Email == command.NewEmail))
            throw new InvalidOperationException("Email already taken");

        user.ChangeEmail(command.NewEmail);

        await _dbContext.SaveChangesAsync();
    }

    public async Task<GetMeResult> GetMeAsync(GetMeQuery query)
    {
        var user = await _dbContext.Users.FindAsync(query.UserId);
        if (user == null)
            throw new InvalidOperationException("User not found");

        return new GetMeResult
        {
            Id = user.Id,
            Email = user.Email,
            Role = user.Role,
            FullName = user.FullName
        };
    }

    public async Task<GetEmailsResult> GetEmails(GetEmailsQuery query)
    {
        var users = await _dbContext.Users
            .Where(u => query.Ids.Contains(u.Id))
            .Select(u => new { u.Id, u.Email })
            .ToListAsync();
        if (users.Count == 0)
            throw new InvalidOperationException("Users not found");

        return new GetEmailsResult
        {
            Emails = users.ToDictionary(x => x.Id, x => x.Email)
        };
    }

    public async Task<List<GetMeResult>> GetManagers()
    {
        var managers = await _dbContext.Users.Where(u => u.Role == Role.Manager || u.Role == Role.HeadManager)
            .Select(u => new GetMeResult
                {
                    Id = u.Id,
                    Email = u.Email,
                    Role = u.Role,
                    FullName = u.FullName
                }
            ).ToListAsync();
        return managers;
    }
    
    public async Task RemoveManager(Guid id)
    {
        var user = await _dbContext.Users.FindAsync(id);

        if (user == null)
            throw new InvalidOperationException("User not found");
        if (user.Role == Role.Applicant || user.Role == Role.Admin)
            throw new InvalidOperationException("You cannot remove the applicant and admin");

        _dbContext.Users.Remove(user);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<GetMeResult> GetManager(Guid id)
    {
        var user = await _dbContext.Users.FindAsync(id);
        if (user == null || user.Role != Role.Manager)
            throw new InvalidOperationException("Manager not found");
        var userDto = new GetMeResult
        {
            Id = user.Id,
            Email = user.Email,
            Role = user.Role,
            FullName = user.FullName
        };
        return userDto;
    }
    
    
    private async Task<bool> EmailExists(string email)
    {
        return await _dbContext.Users.AnyAsync(u => u.Email == email);
    }
}