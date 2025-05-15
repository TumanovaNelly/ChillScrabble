namespace ChillScrabble.Models;

public class Player(string name)
{
    public string Name { get; } = name;
    public int Score { get; private set; }

    public Dictionary<int, Tile> Tiles { get; } = new();

    public void AddTile(Tile tile)
    {
        Tiles[tile.Id] = tile;
    }
    public void AddTiles(HashSet<Tile> tiles)
    {
        foreach (Tile tile in tiles)
            AddTile(tile);
    }

    public Tile GetTile(int tileId)
    {
        var tile = Tiles[tileId];
        Tiles.Remove(tileId);
        return tile;
    }
    
    public void AddPoints(int points) => Score += points;
}