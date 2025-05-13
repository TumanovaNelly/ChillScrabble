using System.Text.Json;
using System.Text.Json.Serialization;
using ChillScrabble.Extensions;
using ChillScrabble.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace ChillScrabble.Controllers;

public class GameController(IMemoryCache memoryCache, IWebHostEnvironment environment) : Controller
{
    private const string GameIdCookieName = "GameId";

    public IActionResult Index(int gameMode)
    {
        var gameId = Guid.NewGuid().ToString();
        var game = new Game();

        memoryCache.Set(gameId, game, new MemoryCacheEntryOptions
        {
            SlidingExpiration = TimeSpan.FromMinutes(30),
            Priority = CacheItemPriority.High,
        });

        Response.Cookies.Append(GameIdCookieName, gameId, new CookieOptions
        {
            Expires = DateTimeOffset.Now.AddHours(1),
            HttpOnly = true,
            Secure = !environment.IsDevelopment() // Правильная проверка окружения
        });

        return View(game);
    }

    [HttpGet]
    public IActionResult GetActivePlayer()
    {
        var game = getGame();
        if (game == null) return NotFound();

        return Json(new { success = true, activePlayer = game.ActivePlayerIndex });
    }

    [HttpGet]
    public IActionResult DealNewTiles(int playerId)
    {
        var game = getGame();
        if (game == null) return NotFound();

        var newTiles = game.DealTiles(playerId);

        // Обновляем время жизни кэша


        return Json(new
        {
            success = true,
            tiles = newTiles
        });
    }

    private Game? getGame()
    {
        if (!Request.Cookies.TryGetValue(GameIdCookieName, out var gameId) || string.IsNullOrEmpty(gameId))
            return null;

        if (!memoryCache.TryGetValue(gameId, out Game game) || game == null)
            return null;

        memoryCache.Set(gameId, game, TimeSpan.FromMinutes(30));

        return game;
    }
}