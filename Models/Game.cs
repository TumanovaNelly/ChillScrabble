using System.Collections.Generic;

namespace ChillScrabble.Models
{
    public class Game
    {
        public List<Player> Players { get; set; } = new List<Player>();
        public List<Tile> Tiles { get; set; } = new List<Tile>();

        public Game()
        {
            
        }
    }
}