namespace ChillScrabble.Models;

public class ChipsBag
{
    private readonly Dictionary<char, int> _lettersCount = new Dictionary<char, int>
    {
        ['А'] = 8, ['Б'] = 2, ['В'] = 4, ['Г'] = 2, ['Д'] = 4, ['Е'] = 9, ['Ж'] = 1, ['З'] = 2,
        ['И'] = 6, ['Й'] = 1, ['К'] = 4, ['Л'] = 4, ['М'] = 3, ['Н'] = 5, ['О'] = 10, ['П'] = 4,
        ['Р'] = 5, ['С'] = 5, ['Т'] = 5, ['У'] = 4, ['Ф'] = 1, ['Х'] = 1, ['Ц'] = 1, ['Ч'] = 1,
        ['Ш'] = 1, ['Щ'] = 1, ['Ъ'] = 1, ['Ы'] = 2, ['Ь'] = 2, ['Э'] = 1, ['Ю'] = 1, ['Я'] = 2
    };

    public int Count = 0;

    private HashSet<Tile> _bag = [];
    public ChipsBag()
    {
        foreach (char letter in _lettersCount.Keys)
        {
            _bag.Add(new Tile(letter));
            ++Count;
        }
    }

    public HashSet<Tile> GiveOutTiles(int count)
    {
        if (count > _bag.Count)
            throw new InvalidOperationException("Cannot give out more tiles than bag contains.");
        
        var random = new Random();
        var deletedTiles = new HashSet<Tile>();

        for (int i = 0; i < count; ++i)
        {
            int index = random.Next(_bag.Count);
            
            var tile = _bag.Skip(index).First();

            _bag.Remove(tile);
            deletedTiles.Add(tile);
            Count--;
        }
        
        return deletedTiles;
    }
}