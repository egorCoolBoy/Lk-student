using Microsoft.AspNetCore.Mvc;
using WebApplication1.Services;

namespace WebApplication1.Controllers;

public class DirectoryController : Controller
{
    private readonly IDirectoriesApi  _directoriesApi;

    public DirectoryController(IDirectoriesApi directoriesApi)
    {
        _directoriesApi = directoriesApi;
    }

    public async Task<IActionResult> GetImportedData()
    {
        var accessToken = HttpContext.Request.Cookies["access_token"];
        if (accessToken == null)
            return RedirectToAction("Login","Account");

        try
        {
            var model = await _directoriesApi.GetImportedData(accessToken);
            return View(model);
        }
        catch (Exception ex)
        {
            ViewBag.GetImport = ex.Message;
            return View();
        }
        
    }
    
    public async Task<IActionResult> Import()
    {
        var accessToken = HttpContext.Request.Cookies["access_token"];
        if (accessToken == null)
            return RedirectToAction("Login","Account");
        
        try
        {
            await _directoriesApi.Import(accessToken);
        }
        catch (Exception ex)
        {
            ViewBag.Import = ex.Message;
        }
        return RedirectToAction("GetImportedData");
    }
}