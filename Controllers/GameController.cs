using Microsoft.AspNetCore.Mvc;

namespace ChillScrabble.Controllers;

public class GameController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}