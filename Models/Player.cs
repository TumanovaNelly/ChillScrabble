namespace ChillScrabble.Models;

public class Player(string name)
{
    public string? Name { get; } = name;
    public int Score { get; set; } = 0;
    
    public HashSet<Tile> Tiles { get; set; } = [];
}