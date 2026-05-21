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
        catch
        {
            ViewBag.GetImport = "Error";
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
        catch
        {
            ViewBag.Import = "Import failed";
        }
        return RedirectToAction("GetImportedData");
    }
}