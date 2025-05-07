namespace ChillScrabble.Models;

public class Game
{
    // потом сделать наследование или что-то подобное (3 класса вместо одного)
    public const int OnlM = 0; // online mode
    public const int OffM = 1; // offline mode
    public const int BotM = 2; // with bot mode    
    
    public int Mode;
    // --------------
        
    public const int PlayersNumber = 2;
    public List<Player> Players { get; } = [];
            
    public ChipsBag Bag = new ChipsBag();
    public PlayBoard Board = new PlayBoard();
            
    public Game(int mode)
    
    {
        Mode = mode;  //////
        
        for (int i = 0; i < PlayersNumber; ++i)
            Players.Add(new Player($"CHILL_GUY_{i}"));

        Players[0].Tiles.Add(new Tile('А'));
    }  
}