using System.Data;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1.Models.VIewModels;
using WebApplication1.Services;

namespace WebApplication1.Controllers;

public class AccountController : Controller
{
    private readonly IUsersServiceApi  _usersService;

    public AccountController(IUsersServiceApi  usersService)
    {
        _usersService = usersService;
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginModel model)
    {
        var request = new LoginRequest()
        {
            Email = model.Email,
            Password = model.Password
        };
        try
        {
            var accessToken = await _usersService.Login(request);
            Response.Cookies.Append("access_token", accessToken);
            return RedirectToAction("GetImportedData", "Directory");
        }
        catch (Exception ex)
        {
            ViewBag.Error = ex.Message;
            return View(model);
        }
    }
    [HttpGet]
    public IActionResult RegisterManager()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> RegisterManager(RegisterModel model)
    {
        var accessToken = HttpContext.Request.Cookies["access_token"];
        if (accessToken == null)
            return RedirectToAction("Login");
        
        var request = new RegisterRequest()
        {
            Email = model.Email,
            Password = model.Password,
            Role = model.Role,
            FullName = model.FullName
        };
        
        try
        {
            var id = await _usersService.RegisterManager(request,accessToken);
            ViewBag.Success = "Менеджер успешно создан";
            return View();
        }
        catch (Exception ex)
        {
            ViewBag.Error = ex.Message;
            return View(model);
        }
            
    }
    
}