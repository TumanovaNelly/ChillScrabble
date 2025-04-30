using ChillScrabble.Models;
using Microsoft.AspNetCore.Mvc;

namespace ChillScrabble.Controllers;

public class GameController : Controller
{
    public IActionResult Index(int gameMode)
    {
        var game = new Game(gameMode);

        return View(game);
    }
}