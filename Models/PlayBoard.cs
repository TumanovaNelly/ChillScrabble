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

    private Tile?[,] _newTiles = new Tile[Size, Size];
    private Tile?[,] _fixedTiles = new Tile[Size, Size];

    public void FixTiles()
    {
        for (int i = 0; i < Size; ++i)
        {
            for (int j = 0; j < Size; ++j)
            {
                _fixedTiles[i, j] = _newTiles[i, j];
                _newTiles[i, j] = null;
            }
        }
    }

    public void PutTile(Tile tile, Tuple<int, int> position)
    {
        _newTiles[position.Item1, position.Item2] = tile;
    }

    public void MoveTile(Tuple<int, int> fromPosition, Tuple<int, int> toPosition)
    {
        (_newTiles[fromPosition.Item1, fromPosition.Item2], _newTiles[toPosition.Item1, toPosition.Item2]) = (
            _newTiles[toPosition.Item1, toPosition.Item2], _newTiles[fromPosition.Item1, fromPosition.Item2]);
    }

    public Tile GetTile(Tuple<int, int> position)
    {
        var removedTile = _newTiles[position.Item1, position.Item2];
        if (removedTile is null)
            throw new NullReferenceException("Tile is empty");
        return removedTile;
    }
}