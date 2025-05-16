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
    public const int OnlM = 0; // online mode
    public const int OffM = 1; // offline mode
    public const int BotM = 2; // with bot mode    
    public const int PlayersNumber = 2;
    
    public int Mode { get; }
    public List<Player> Players { get; } = [];
    public int? ActivePlayerIndex { get; set; }
    public ChipsBag Bag { get; } = new();
    public PlayBoard Board { get; } = new();
    private readonly WordValidationService _wordValidator;

    // Исправленный конструктор
    public Game(WordValidationService wordValidator)
    {
        _wordValidator = wordValidator;
        Mode = OffM;

        for (int i = 0; i < PlayersNumber; ++i)
            Players.Add(new Player($"CHILL_GUY_{i}"));
    }

    // Обновлённый метод валидации
    public ValidationResult ValidateBoard()
    {
        var newWords = Board.GetNewWords();
        
        foreach (var word in newWords)
        {
            bool isEnglish = WordValidationService.DetermineLanguage(word);
            if (!_wordValidator.ValidateWord(word, isEnglish))
            {
                return new ValidationResult(false, $"Слово '{word}' не существует в словаре");
            }
        }
        return new ValidationResult(true);
    }

    public void AssignNextActivePlayer()
    {
        if (ActivePlayerIndex is null) return;
        ActivePlayerIndex = (ActivePlayerIndex + 1) % PlayersNumber;
    }

    public HashSet<Tile> DealTiles(int playerId)
    {
        var newTiles = Bag.GiveOutTiles(7 - Players[playerId].Tiles.Count);
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
}