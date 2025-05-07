namespace ChillScrabble.Models;

public class ChipsBag
{
    public int Count = 0;
    
    private readonly Dictionary<char, int> _lettersCount = new Dictionary<char, int>
    {
        ['А'] = 8, ['Б'] = 2, ['В'] = 4, ['Г'] = 2, ['Д'] = 4, ['Е'] = 9, ['Ж'] = 1, ['З'] = 2,
        ['И'] = 6, ['Й'] = 1, ['К'] = 4, ['Л'] = 4, ['М'] = 3, ['Н'] = 5, ['О'] = 10, ['П'] = 4,
        ['Р'] = 5, ['С'] = 5, ['Т'] = 5, ['У'] = 4, ['Ф'] = 1, ['Х'] = 1, ['Ц'] = 1, ['Ч'] = 1,
        ['Ш'] = 1, ['Щ'] = 1, ['Ъ'] = 1, ['Ы'] = 2, ['Ь'] = 2, ['Э'] = 1, ['Ю'] = 1, ['Я'] = 2,
           
        ['A'] = 9, ['B'] = 2, ['C'] = 2, ['D'] = 4, ['E'] = 12, ['F'] = 2, ['G'] = 3, ['H'] = 2,
        ['I'] = 9, ['J'] = 1, ['K'] = 1, ['L'] = 4, ['M'] = 2, ['N'] = 6, ['O'] = 8, ['P'] = 2,
        ['Q'] = 1, ['R'] = 6, ['S'] = 4, ['T'] = 6, ['U'] = 4, ['V'] = 2, ['W'] = 2, ['X'] = 1,
        ['Y'] = 2, ['Z'] = 1
    };

    private HashSet<Tile> _bag = [];
    
    public ChipsBag()
    {
        foreach (char letter in _lettersCount.Keys)
            for (int i = 0; i < _lettersCount[letter]; ++i)
            {
                _bag.Add(new Tile(letter));
                ++Count;
            }
    }

    public HashSet<Tile> GiveOutTiles(int count)
    {
        throw new NotImplementedException("Метод для удаления фишек из мешка.");
    }
}