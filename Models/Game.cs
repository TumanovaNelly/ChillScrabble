using System.Collections.Generic;

namespace ChillScrabble.Models
{
    public class Game
    {
        public List<Player> Players { get; } = new List<Player>();
        public int Mode;
        
        private List<Tile> _bag = new List<Tile>();
        private const int _players_number = 2;

        private const int _board_size = 15;
        public Tile[,] Board { get; set; } = new Tile[_board_size, _board_size];

        public const int Mid = -1; // middle 
        public const int Mpt = 0;  // empty
        public const int Lx2 = 2;  // letter value x2
        public const int Lx3 = 3;  // letter value x3
        public const int Wx2 = 22; // word value x2
        public const int Wx3 = 33; // word value x3

        public const int OnlM = 0; // online mode
        public const int OffM = 1; // offline mode
        public const int BotM = 2; // with bot mode

        public readonly int[,] BoardBonus = new int[_board_size, _board_size]
        {
            { Wx3, Mpt, Mpt, Lx2, Mpt, Mpt, Mpt, Wx3, Mpt, Mpt, Mpt, Lx2, Mpt, Mpt, Wx3 },
            { Mpt, Wx2, Mpt, Mpt, Mpt, Lx3, Mpt, Mpt, Mpt, Lx3, Mpt, Mpt, Mpt, Wx2, Mpt },
            { Mpt, Mpt, Wx2, Mpt, Mpt, Mpt, Lx2, Mpt, Lx2, Mpt, Mpt, Mpt, Wx2, Mpt, Mpt },
            { Lx2, Mpt, Mpt, Wx2, Mpt, Mpt, Mpt, Lx2, Mpt, Mpt, Mpt, Wx2, Mpt, Mpt, Mpt },
            { Mpt, Mpt, Mpt, Mpt, Wx2, Mpt, Mpt, Mpt, Mpt, Mpt, Wx2, Mpt, Mpt, Mpt, Mpt },
            { Mpt, Lx3, Mpt, Mpt, Mpt, Mpt, Mpt, Mpt, Mpt, Mpt, Mpt, Mpt, Mpt, Lx3, Mpt },
            { Mpt, Mpt, Lx2, Mpt, Mpt, Mpt, Lx2, Mpt, Lx2, Mpt, Mpt, Mpt, Lx2, Mpt, Mpt },
            { Wx3, Mpt, Mpt, Lx2, Mpt, Mpt, Mpt, Mid, Mpt, Mpt, Mpt, Lx2, Mpt, Mpt, Wx3 },
            { Mpt, Mpt, Lx2, Mpt, Mpt, Mpt, Lx2, Mpt, Lx2, Mpt, Mpt, Mpt, Lx2, Mpt, Mpt },
            { Mpt, Lx3, Mpt, Mpt, Mpt, Mpt, Mpt, Mpt, Mpt, Mpt, Mpt, Mpt, Mpt, Lx3, Mpt },
            { Mpt, Mpt, Mpt, Mpt, Wx2, Mpt, Mpt, Mpt, Mpt, Mpt, Wx2, Mpt, Mpt, Mpt, Mpt },
            { Lx2, Mpt, Mpt, Wx2, Mpt, Mpt, Mpt, Lx2, Mpt, Mpt, Mpt, Wx2, Mpt, Mpt, Mpt },
            { Mpt, Mpt, Wx2, Mpt, Mpt, Mpt, Lx2, Mpt, Lx2, Mpt, Mpt, Mpt, Wx2, Mpt, Mpt },
            { Mpt, Wx2, Mpt, Mpt, Mpt, Lx3, Mpt, Mpt, Mpt, Lx3, Mpt, Mpt, Mpt, Wx2, Mpt },
            { Wx3, Mpt, Mpt, Lx2, Mpt, Mpt, Mpt, Wx3, Mpt, Mpt, Mpt, Lx2, Mpt, Mpt, Wx3 },
        };

        public Game(int mode)
        {
            for (int i = 0; i < 10; i++)
            {
                _bag.Add(new Tile('А'));
            }
            
            Mode = mode;
            for (int i = 0; i < _players_number; i++)
            {
                Players.Add(new Player($"CHILL_GUY_{i}"));
            }
        }
    }
}