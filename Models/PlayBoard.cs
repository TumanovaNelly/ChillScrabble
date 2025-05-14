namespace ChillScrabble.Models;

[Serializable]
public class PlayBoard
{
    public const int Size = 15;

    public const int Mid = -1; // middle 
    public const int Mpt = 0; // empty
    public const int Lx2 = 2; // letter value x2
    public const int Lx3 = 3; // letter value x3
    public const int Wx2 = 22; // word value x2
    public const int Wx3 = 33; // word value x3

    public static int[,] Bonuses { get; } =
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
        { Wx3, Mpt, Mpt, Lx2, Mpt, Mpt, Mpt, Wx3, Mpt, Mpt, Mpt, Lx2, Mpt, Mpt, Wx3 }
    };

    private int _tilesOnBoard = 0;
    
    public bool IsBoardEmpty() => _tilesOnBoard == 0;
}