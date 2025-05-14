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
    
    [HttpPost]
    public IActionResult HandleTileMove([FromBody] TileMoveRequest request)
    {
        var game = getGame();
        if (game == null) return NotFound();
        
        try
        {
            switch (request.FromType, request.ToType)
            {
                case ("slot", "board-cell"):
                    Console.WriteLine("Перемещение со слота на поле");
                    if (game.Board.IsBoardEmpty())
                        game.ActivePlayerIndex = request.PlayerId;
                    break;
                case ("board-cell", "slot"):
                    Console.WriteLine("Перемещение с поля в слот");
                    if (game.Board.IsBoardEmpty())
                        game.ActivePlayerIndex = null;
                    break;
                case ("board-cell", "board-cell"):
                    Console.WriteLine("Перемещения на поле");
                    break;
                default:
                    return Json(new { success = false, message = "Недопустимый тип перемещения" });
            }
            
            return Json(new { success = true });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = ex.Message });
        }
    }
    
    public class TileMoveRequest
    {
        public int TileId { get; set; }
        public string FromType { get; set; } = string.Empty;  // "slot" или "board-cell"
        public string ToType { get; set; } = string.Empty;   // "slot" или "board-cell"
        public int PlayerId { get; set; }
        public string Letter { get; set; } = string.Empty;
        public int Value { get; set; }
        public Position FromPosition { get; set; } = new();
        public Position ToPosition { get; set; } = new();
    }

    public class Position
    {
        public int? X { get; set; }
        public int? Y { get; set; }
    }

    [HttpGet]
    public IActionResult GetActivePlayer()
    {
        var game = getGame();
        if (game == null) return NotFound();

        return Json(new { success = true, active = game.ActivePlayerIndex });
    }

    [HttpGet]
    public IActionResult DealNewTiles(int playerId)
    {
        var game = getGame();
        if (game == null) return NotFound();

        var newTiles = game.DealTiles(playerId);

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