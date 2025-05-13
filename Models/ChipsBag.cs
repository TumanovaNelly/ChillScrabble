using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace ChillScrabble.Models;

public class ChipsBag
{
    [JsonIgnore]
    private static readonly IReadOnlyDictionary<char, int> LettersCount = new ReadOnlyDictionary<char, int>(
        new Dictionary<char, int>
        {
            ['А'] = 8, ['Б'] = 2, ['В'] = 4, ['Г'] = 2, ['Д'] = 4, ['Е'] = 9, ['Ж'] = 1, ['З'] = 2,
            ['И'] = 6, ['Й'] = 1, ['К'] = 4, ['Л'] = 4, ['М'] = 3, ['Н'] = 5, ['О'] = 10, ['П'] = 4,
            ['Р'] = 5, ['С'] = 5, ['Т'] = 5, ['У'] = 4, ['Ф'] = 1, ['Х'] = 1, ['Ц'] = 1, ['Ч'] = 1,
            ['Ш'] = 1, ['Щ'] = 1, ['Ъ'] = 1, ['Ы'] = 2, ['Ь'] = 2, ['Э'] = 1, ['Ю'] = 1, ['Я'] = 2
        });
    
    private int Count { get; set; }
    private HashSet<Tile> Bag { get; } = [];

    public ChipsBag()
    {
        foreach (char letter in LettersCount.Keys)
            for (int i = 0; i < LettersCount[letter]; ++i)
            {
                Bag.Add(new Tile(Count, letter));
                ++Count;
            }
    }

    public HashSet<Tile> GiveOutTiles(int count)
    {
        if (Bag.Count < count) return [];
        
        var outTiles = new HashSet<Tile>();
        var bagArray = Bag.ToArray();

        for (int i = 0; i < count; ++i)
        {
            Random random = new Random();
            int randomIndex = random.Next(0, bagArray.Length);
            outTiles.Add(bagArray[randomIndex]);
            Bag.Remove(bagArray[randomIndex]);
            --Count;
        }
        return outTiles;
    }
}