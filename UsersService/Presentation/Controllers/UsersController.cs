using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UsersService;
using UsersService.Application.Users.Queries;
using UsersService.Application.Users.UsersCommands;
using UsersService.Domain;
using UsersService.Presentation.DTO;
using UsersService.Presentation.DTO.ChangeEmail;
using UsersService.Presentation.DTO.ChangePassword;
using UsersService.Presentation.DTO.GetEmails;
using UsersService.Presentation.DTO.Login;
using UsersService.Presentation.DTO.RefreshToken;
using Contracts;
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
            Role = request.Role,
            FullName = request.FullName
        };
        
        var result = await _usersService.RegisterAsync(command);

        var responce = new RegisterResponce()
        {
            UserId = result.UserId,
        };
        
        return Ok(responce);

    }
    [Authorize(Roles = "Admin")]
    [HttpPost("register/manager")]
    public async Task<IActionResult> Post(RegisterManagerRequest request)
    {
        var command = new RegisterManagerCommand()
        {
            Email = request.Email,
            Password = request.Password,
            Role = Role.Manager,
            FullName = request.FullName
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
            Email = request.Email,
            Password = request.Password
        };

        var result = await _usersService.LoginAsync(command);
        
        Response.Cookies.Append("refreshToken", result.RefreshToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = false,
            SameSite = SameSiteMode.Strict,
            Expires = DateTime.UtcNow.AddDays(30)
        });

        var response = new LoginResponce()
        {
            UserId = result.UserId,
            Email = result.Email,
            Role = result.Role,
            AccessToken = result.AccessToken
        };

        return Ok(response);
    }
    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        var refreshToken = Request.Cookies["refreshToken"];

        if (!string.IsNullOrEmpty(refreshToken))
        {
            await _usersService.LogoutAsync(new LogoutCommand
            {
                RefreshToken = refreshToken
            });
        }
        Response.Cookies.Delete("refreshToken");

        return Ok();
    }
    
    [Authorize]
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
    [Authorize]
    [HttpPost("change-email")]
    public async Task<IActionResult> ChangeEmail(ChangeEmailRequest request)
    {
        var userId = GetUserId();


        var command = new ChangeEmailCommand
        {
            UserId = userId,
            NewEmail = request.NewEmail,
            Password = request.Password
        };
        await _usersService.ChangeEmailAsync(command);
        return Ok();
    }
    
    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh()
    {
        var refreshToken = Request.Cookies["refreshToken"];

        if (string.IsNullOrEmpty(refreshToken))
            return Unauthorized();

        var result = await _usersService.RefreshTokenAsync(new RefreshTokenCommand
        {
            RefreshToken = refreshToken
        });
        
        Response.Cookies.Append("refreshToken", result.RefreshToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTime.UtcNow.AddDays(30)
        });

        return Ok(new RefreshTokenResponce()
        {
            AccessToken = result.AccessToken
        });
    }
    [Authorize]
    [HttpGet("me")]
    public async Task<IActionResult> GetMe()
    {
        var query = new GetMeQuery()
        {
            UserId = GetUserId()
        };
        var result = await _usersService.GetMeAsync(query);

        var responce = new  GetMeResponce()
        {
            UserId = result.Id,
            Email = result.Email,
            Role = result.Role,
            FullName = result.FullName
        };
        return Ok(responce);
    }
    [HttpGet("users/emails")]
    public async Task<IActionResult> GetUserEmails(GetEmailsRequest request)
    {
        var query = new GetEmailsQuery()
        {
            Ids = request.Ids,  
        };

        var result = await _usersService.GetEmails(query);

        var responce = new GetEmailsResponce()
        {
            Emails = result.Emails,
        };
        return Ok(responce);
    }
    [Authorize(Roles = "HeadManager,Admin")]
    [HttpGet("Managers")]
    public async Task<IActionResult> GetManagers()
    {
        return Ok(await _usersService.GetManagers());
    }
    
    [HttpGet("manager/{id}")]
    public async Task<IActionResult> GetManager(Guid id)
    {
        return Ok(await _usersService.GetManager(id));
    }
    
    [Authorize(Roles = "Admin")]
    [HttpDelete("Manager/{id}")]
    public async Task<IActionResult> DeleteManager(Guid id)
    {
        await _usersService.RemoveManager(id);
        return NoContent();
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("Manager/{id}")]
    public async Task<IActionResult> UpdateManagerName(ChangeManagerName request)
    {
        var res = await _usersService.UpdateManagerName(request);
        return Ok(res);
        
    }
    private Guid GetUserId()
    {
        var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(id))
            throw new UnauthorizedAccessException("Missing name claim");
        return Guid.Parse(id);
    }

}