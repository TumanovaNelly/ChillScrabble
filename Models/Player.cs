namespace ChillScrabble.Models;

public class Player(string name)
{
    public string Name { get; } = name;
    public int Score { get; private set; }

    public Dictionary<int, Tile> Tiles { get; } = new();

    public void AddTiles(HashSet<Tile> tiles)
    {
        foreach (Tile tile in tiles)
            Tiles[tile.Id] = tile;
    }
    
    public void AddPoints(int points) => Score += points;
}