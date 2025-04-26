using Microsoft.AspNetCore.Mvc;

namespace ChillScrabble.Controllers;

public class SettingsController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}