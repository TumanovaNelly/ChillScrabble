using Microsoft.AspNetCore.Mvc;

namespace ChillScrabble.Controllers;

public class RulesController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}