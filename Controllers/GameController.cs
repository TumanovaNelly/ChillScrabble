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
                    if (request.ToPosition.Row is null || request.ToPosition.Column is null)
                        throw new ArgumentException("Координаты X или Y не могут быть null.");

                    var toPosition = Tuple.Create(request.ToPosition.Row.Value, request.ToPosition.Column.Value);
                    game.FromSlotToBoardMove(request.PlayerId, request.TileId, toPosition);
                    if (game.Board.IsBoardEmpty())
                        game.ActivePlayerIndex = request.PlayerId;
                    break;
                case ("board-cell", "slot"):
                    if (request.FromPosition.Row is null || request.FromPosition.Column is null)
                        throw new ArgumentException("Координаты X или Y не могут быть null.");

                    var fromPosition = Tuple.Create(request.FromPosition.Row.Value, request.FromPosition.Column.Value);
                    game.FromBoardToSlotMove(request.PlayerId, fromPosition);
                    if (game.Board.IsBoardEmpty())
                        game.ActivePlayerIndex = null;
                    break;
                case ("board-cell", "board-cell"):
                    if (request.FromPosition.Row is null || request.FromPosition.Column is null)
                        throw new ArgumentException("Координаты X или Y не могут быть null.");
                    
                    if (request.ToPosition.Row is null || request.ToPosition.Column is null)
                        throw new ArgumentException("Координаты X или Y не могут быть null.");
                    
                    var toPos = Tuple.Create(request.ToPosition.Row.Value, request.ToPosition.Column.Value);
                    var fromPos = Tuple.Create(request.FromPosition.Row.Value, request.FromPosition.Column.Value);
                    
                    game.OnBoardMove(fromPos, toPos);
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
        public Position FromPosition { get; set; } = new();
        public Position ToPosition { get; set; } = new();
    }

    public class Position
    {
        public int? Row { get; set; }
        public int? Column { get; set; }
    }
    
    [HttpPost]
    public IActionResult ValidateBoard()
    {
        try
        {
            var game = getGame();
            if (game == null) return NotFound();
        
            // Проверяем корректность текущего состояния поля
            var validationResult = game.ValidateBoard();
        
            if (!validationResult.Success)
            {
                return Json(new { 
                    success = false, 
                    message = validationResult.ErrorMessage 
                });
            }
        
            return Json(new { success = true });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { 
                success = false, 
                message = ex.Message 
            });
        }
    }

    [HttpPost]
    public IActionResult SwitchActivePlayer()
    {
        try
        {
            var game = getGame();
            if (game == null) return NotFound();
            
            var oldActive = game.ActivePlayerIndex;
        
            game.AssignNextActivePlayer();
        
            return Json(new { 
                success = true, 
                oldActivePlayer = oldActive,
                newActivePlayer = game.ActivePlayerIndex 
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { 
                success = false, 
                message = ex.Message 
            });
        }
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