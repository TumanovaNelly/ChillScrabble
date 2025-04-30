namespace ChillScrabble.Models
{
    public class Player(string name)
    {
        public string? Name { get; } = name;
        public int Score { get; set; } = 0;
        public List<Tile> Tiles { get; set; } = new List<Tile>();
    }
}