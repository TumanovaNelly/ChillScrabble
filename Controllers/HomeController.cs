using Microsoft.AspNetCore.Mvc;
using ChillScrabble.Models;

namespace ChillScrabble.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}