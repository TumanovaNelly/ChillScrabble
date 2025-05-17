using Microsoft.AspNetCore.Mvc;
using ChillScrabble.Services;


namespace ChillScrabble.Models;

public class ValidationResult
{
    public ValidationResult(bool result, string message = "")
    {
        Success = result;
        if (!result) 
            ErrorMessage = message;
    }
    
    public bool Success { get; set; }
    public string ErrorMessage { get; set; } = string.Empty;
}

public class Game
{ 
    private readonly WordValidationService _wordValidator = new WordValidationService();

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
    public PlayBoard Board { get; } = new();

    public Game(int mode)
    {
        Mode = mode; //////

        for (int i = 0; i < PlayersNumber; ++i)
            Players.Add(new Player($"CHILL_GUY_{i}"));
    }

    public void AssignNextActivePlayer()
    {
        ActivePlayerIndex = (ActivePlayerIndex + 1) % PlayersNumber;
    }

    public HashSet<Tile> DealTiles(int playerId)
    {
        var newTiles = Bag.GiveOutTiles(7 - Players[playerId].Tiles.Count); // заменить 7
        Players[playerId].AddTiles(newTiles);
        return newTiles;
    }

    public void FromSlotToBoardMove(int playerId, int tileId, Tuple<int, int> position)
    {
        var movedTile = Players[playerId].GetTile(tileId);
        
        Board.PutTile(movedTile, position);
    }

    public void FromBoardToSlotMove(int playerId, Tuple<int, int> position)
    {
        var movedTile = Board.GetTile(position);
        Players[playerId].AddTile(movedTile);
    }

    public void OnBoardMove(Tuple<int, int> fromPosition, Tuple<int, int> toPosition)
    {
        Board.MoveTile(fromPosition, toPosition);
    }

    public ValidationResult ValidateBoard()
    {
        return new ValidationResult(true);
    }
    
}