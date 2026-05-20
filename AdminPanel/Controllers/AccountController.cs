using System.Data;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1.Services;

namespace WebApplication1.Controllers;

public class AccountController : Controller
{
    private readonly IUsersServiceApi  _usersService;

    public AccountController(IUsersServiceApi  usersService)
    {
        _usersService = usersService;
    }

   
    public IActionResult Login()
    {
        return View();
    }

    
    public async Task<IActionResult> Login(LoginRequest model)
    {
        var request = new LoginRequest
        {
            Login = model.Login,
            Password = model.Password
        };

        var accessToken = await _usersService.Login(request);
        
        Response.Cookies.Append("access_token", accessToken);
        throw new RowNotInTableException();
    }
    
}