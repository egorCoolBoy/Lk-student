using Microsoft.EntityFrameworkCore;
using UsersService.Application.Hash;
using UsersService.Application.Users.Queries;
using UsersService.Application.Users.UsersCommands;
using UsersService.Application.Users.UsersResults;
using UsersService.Domain.Entities;
using UsersService.Infrastructure.AppDbContext;

namespace UsersService;

public class UserService : IUsersService
{
    private readonly AppDbContext _dbContext;
    private readonly IPasswordHash _passwordHasher;

    public UserService(AppDbContext dbContext, IPasswordHash passwordHasher)
    {
        _dbContext = dbContext;
        _passwordHasher = passwordHasher;
    }

    public async Task<RegisterResult> RegisterAsync(RegisterCommand command)
    {
        if (await EmailExists(command.Email))
        {
            throw new InvalidOperationException("Email is already exists");
        }
        
        var passwordHash = _passwordHasher.Hash(command.Password);
        
        var user = new User(command.Email,passwordHash,command.Role);
        
        await _dbContext.Users.AddAsync(user);
        await _dbContext.SaveChangesAsync();

        
        return new RegisterResult
        {
            UserId = user.Id
        };
    }
    public Task<RegisterResult> RegisterManagerAsync(RegisterManagerCommand command)
    {
        throw new NotImplementedException();
    }

    public Task<LoginResult> LoginAsync(LoginCommand command)
    {
        throw new NotImplementedException();
    }

    public Task<RefreshTokenResult> RefreshTokenAsync(RefreshTokenCommand command)
    {
        throw new NotImplementedException();
    }

    public Task ChangePasswordAsync(ChangePasswordCommand command)
    {
        throw new NotImplementedException();
    }

    public Task ChangeEmailAsync(ChangeEmailCommand command)
    {
        throw new NotImplementedException();
    }

    public Task<GetMeResult> GetMeAsync(GetMeQuery query)
    {
        throw new NotImplementedException();
    }

    public Task<GetEmailsResult> GetEmails(GetEmailsQuery query)
    {
        throw new NotImplementedException();
    }
    
    
    private async Task<bool> EmailExists(string email)
    {
        return await _dbContext.Users.AnyAsync(u => u.Email == email);
    }
}