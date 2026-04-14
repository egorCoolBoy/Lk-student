using Microsoft.AspNetCore.Mvc;
using UsersService;
using UsersService.Application.Users.UsersCommands;
using UsersService.Presentation.DTO;
using UsersService.Presentation.DTO.ChangeEmail;
using UsersService.Presentation.DTO.ChangePassword;
using UsersService.Presentation.DTO.Login;

namespace Presentation.Controllers;

[ApiController]
[Route("api/auth")]
public class UsersController : ControllerBase
{
    private readonly IUsersService _usersService;

    public UsersController(IUsersService usersService)
    {
        _usersService = usersService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Post(RegisterRequest request)
    {
        var command = new RegisterCommand()
        {
            Email = request.Email,
            Password = request.Password,
            Role = request.Role
        };
        
        var result = await _usersService.RegisterAsync(command);

        var responce = new RegisterResponce()
        {
            UserId = result.UserId,
        };
        
        return Ok(responce);

    }
    
    [HttpPost("register/manager")]
    public async Task<IActionResult> Post(RegisterManagerRequest request)
    {
        var command = new RegisterManagerCommand()
        {
            Email = request.Email,
            Password = request.Password,
            Role = request.Role
        };
        
        var result = await _usersService.RegisterManagerAsync(command);
        
        var responce = new RegisterResponce()
        {
            UserId = result.UserId,
        };
        
        return Ok(responce);
    }
    
    
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var command = new LoginCommand()
        {
            UserId = GetUserId(),
            Email = request.Email,
            Password = request.Password
        };

        var result = await _usersService.LoginAsync(command);

        var responce = new LoginResponce()
        {
            UserId = result.UserId,
            Email = result.Email,
            Role = result.Role,
            AccessToken = result.AccessToken,
            RefreshToken = result.RefreshToken
        };
        return Ok(responce);
    }
    
    [HttpPost("logout")]
    public async Task<IActionResult> Logout(LogoutCommand command)
    {
        await _usersService.LogoutAsync(command);
        return Ok();
    }
    
    
    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePassword(ChangePasswordRequest request)
    {
        var command = new ChangePasswordCommand()
        {
            UserId = GetUserId(),
            CurrentPassword = request.CurrentPassword,
            NewPassword = request.NewPassword
        };

        await _usersService.ChangePasswordAsync(command);

        return NoContent();
    }
    
    [HttpPost("change-email")]
    public async Task<IActionResult> ChangeEmail(ChangeEmailRequest request)
    {
        var userId = Guid.Parse(User.FindFirst("id").Value);


        var command = new ChangeEmailCommand
        {
            UserId = userId,
            NewEmail = request.NewEmail,
            Password = request.Password
        };
        await _usersService.ChangeEmailAsync(command);
        return Ok();
    }
    
    
    
    private Guid GetUserId()
    {
        return Guid.Parse(User.FindFirst("sub")?.Value?? User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);
    }

}