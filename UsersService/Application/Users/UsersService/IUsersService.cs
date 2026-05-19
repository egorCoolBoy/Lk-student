using UsersService.Application.Users.Queries;
using UsersService.Application.Users.UsersCommands;
using UsersService.Application.Users.UsersResults;

namespace UsersService;

public interface IUsersService
{
    public Task<RegisterResult> RegisterAsync(RegisterCommand command);
    public Task<RegisterResult> RegisterManagerAsync(RegisterManagerCommand command);

    public Task<LoginResult> LoginAsync(LoginCommand command);
    public Task LogoutAsync(LogoutCommand command);

    public Task<RefreshTokenResult> RefreshTokenAsync(RefreshTokenCommand command);

    public Task ChangePasswordAsync(ChangePasswordCommand command);

    public Task ChangeEmailAsync(ChangeEmailCommand command);
    public Task<GetMeResult>  GetMeAsync(GetMeQuery query);
    public Task<GetEmailsResult> GetEmails(GetEmailsQuery query);
    public Task<List<GetMeResult>> GetManagers();
    public Task RemoveManager(Guid id);
    public Task<GetMeResult> GetManager(Guid id);

}