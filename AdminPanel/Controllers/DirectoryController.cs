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
        var result = await _directoriesApi.GetImportedData();
        return View(result);
    }
    
    public async Task<IActionResult> Import()
    {
        await _directoriesApi.Import();

        return RedirectToAction("GetImportedData");
    }
}