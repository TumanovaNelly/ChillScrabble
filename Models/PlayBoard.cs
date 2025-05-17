using System.Text; // Добавьте в начало файла

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
    public List<string> GetNewWords()
    {
        var words = new HashSet<string>();
        var visited = new bool[Size, Size];

        // Сканируем только новые плитки
        for (int i = 0; i < Size; i++)
        {
            for (int j = 0; j < Size; j++)
            {
                if (_newTiles[i, j] != null && !visited[i, j])
                {
                    // Проверяем горизонтальные слова
                    var horizontalWord = GetWordInDirection(i, j, 0, 1, visited);
                    if (horizontalWord.Length >= 2) words.Add(horizontalWord);

                    // Проверяем вертикальные слова
                    var verticalWord = GetWordInDirection(i, j, 1, 0, visited);
                    if (verticalWord.Length >= 2) words.Add(verticalWord);
                }
            }
        }
        return words.ToList();
    }

    private string GetWordInDirection(int row, int col, int dr, int dc, bool[,] visited)
    {
        // Идем влево/вверх до начала слова
        while (row >= 0 && col >= 0 && 
               (HasTile(row - dr, col - dc) && 
                !visited[row - dr, col - dc]))
        {
            row -= dr;
            col -= dc;
        }

        // Собираем все буквы в направлении
        var word = new StringBuilder();
        while (row < Size && col < Size && HasTile(row, col))
        {
            word.Append(GetLetter(row, col));
            visited[row, col] = true;
            row += dr;
            col += dc;
        }
        return word.ToString().ToUpper();
    }

    public int CountPoints()
    {
        _tilesOnBoard = 1;
        var points = 0;
        foreach (var tile in _newTiles)
            if (tile != null) 
                points += tile.Value;
        
        return points;
    }

    private bool HasTile(int row, int col)
    {
        if (row < 0 || col < 0 || row >= Size || col >= Size) return false;
        return _newTiles[row, col] != null || _fixedTiles[row, col] != null;
    }

    private char GetLetter(int row, int col)
    {
        var tile = _newTiles[row, col] ?? _fixedTiles[row, col];
        return tile?.Letter ?? ' ';
    }
}

