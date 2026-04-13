using Microsoft.AspNetCore.Mvc;
using UsersService;
using UsersService.Application.Users.UsersCommands;
using UsersService.Presentation.DTO;

namespace Presentation.Controllers;

[ApiController]
[Route("api")]
public class UsersController : ControllerBase
{
    private readonly IUsersService _usersService;

    public UsersController(IUsersService usersService)
    {
        _usersService = usersService;
    }

    [HttpPost]
    public async Task<IActionResult> Post(RegisterRequest request)
    {
        var command = new RegisterCommand()
        {
            Email = request.Email,
            Password = request.Password,
            Role = request.Role
        };
        
        var result = await _usersService.RegisterAsync(command);
        
        return Ok(result);
    }

}