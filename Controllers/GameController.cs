using ChillScrabble.Models;
using Microsoft.AspNetCore.Mvc;

namespace ChillScrabble.Controllers;

public class GameController : Controller
{
    private Game? _game;

    public IActionResult Index(int gameMode)
    {
        _game = new Game(gameMode);

        return View(_game);
    }

    [HttpPost]
    public IActionResult MoveTile([FromBody] TileMoveModel model)
    {
        // Удаляем фишку из рук игрока
        var player = _game.Players.FirstOrDefault(p => p.Tiles.Any(t => t.Letter == model.Letter));
        if (player != null)
        {
            var tile = player.Tiles.FirstOrDefault(t => t.Letter == model.Letter);
            
            if (tile != null)
            {
                player.Tiles.Remove(tile);

                // Размещаем фишку на игровом поле
                _game.Board.NewTiles[model.Row, model.Col] = tile;

                return Json(new { success = true });
            }
        }


        return Json(new { success = false });
    }

    public class TileMoveModel
    {
        public char Letter { get; set; }
        public int Row { get; set; }
        public int Col { get; set; }
    }
}