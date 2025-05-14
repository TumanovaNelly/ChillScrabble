using Microsoft.AspNetCore.Mvc;

namespace ChillScrabble.Models;

[Serializable]
public class Game
{
    // потом сделать наследование или что-то подобное (3 класса вместо одного)
    public const int OnlM = 0; // online mode
    public const int OffM = 1; // offline mode
    public const int BotM = 2; // with bot mode    

    public const int PlayersNumber = 2;
    public int Mode { get; }
// --------------

    public List<Player> Players { get; } = [];

    public int? ActivePlayerIndex { get; set; }

    public ChipsBag Bag { get; } = new();
    public PlayBoard Board { get; private set; } = new();

    public Game()
    {
        Mode = 1; //////

        for (int i = 0; i < PlayersNumber; ++i)
            Players.Add(new Player($"CHILL_GUY_{i}"));
    }

    public void AssignNextActivePlayer()
    {
        if (ActivePlayerIndex is null) 
            throw new ArgumentNullException();
        ActivePlayerIndex = (ActivePlayerIndex + 1) % PlayersNumber;
    }

    public HashSet<Tile> DealTiles(int playerId)
    {
        var newTiles = Bag.GiveOutTiles(7 - Players[playerId].Tiles.Count); // заменить 7
        Players[playerId].AddTiles(newTiles);
        return newTiles;
    }
}