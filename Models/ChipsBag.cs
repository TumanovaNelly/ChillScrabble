using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace ChillScrabble.Models;

public class ChipsBag
{
    public static IReadOnlyDictionary<char, int> EnglishLettersCount { get; } = 
        new ReadOnlyDictionary<char, int>(
            new Dictionary<char, int>
            {
                ['A'] = 9, ['B'] = 2, ['C'] = 2, ['D'] = 4, ['E'] = 12, ['F'] = 2, 
                ['G'] = 3, ['H'] = 2, ['I'] = 9, ['J'] = 1, ['K'] = 1, ['L'] = 4, 
                ['M'] = 2, ['N'] = 6, ['O'] = 8, ['P'] = 2, ['Q'] = 1, ['R'] = 6, 
                ['S'] = 4, ['T'] = 6, ['U'] = 4, ['V'] = 2, ['W'] = 2, ['X'] = 1, 
                ['Y'] = 2, ['Z'] = 1
            });

    [JsonIgnore]
    public static IReadOnlyDictionary<char, int> RussianLettersCount { get; } = 
        new ReadOnlyDictionary<char, int>(
            new Dictionary<char, int>
            {
                ['А'] = 8, ['Б'] = 2, ['В'] = 4, ['Г'] = 2, ['Д'] = 4, ['Е'] = 9, 
                ['Ж'] = 1, ['З'] = 2, ['И'] = 6, ['Й'] = 1, ['К'] = 4, ['Л'] = 4, 
                ['М'] = 3, ['Н'] = 5, ['О'] = 10, ['П'] = 4, ['Р'] = 5, ['С'] = 5, 
                ['Т'] = 5, ['У'] = 4, ['Ф'] = 1, ['Х'] = 1, ['Ц'] = 1, ['Ч'] = 1, 
                ['Ш'] = 1, ['Щ'] = 1, ['Ъ'] = 1, ['Ы'] = 2, ['Ь'] = 2, ['Э'] = 1, 
                ['Ю'] = 1, ['Я'] = 2
            });

    // Остальная часть класса без изменений
    // ...


    private int Count { get; set; }
    private HashSet<Tile> Bag { get; } = [];
    private readonly Random _random = new();

    public ChipsBag()
    {
        // В текущей версии используем только английские буквы
        GenerateEnglishTiles();
        
        // Для будущего использования можно раскомментировать:
        // GenerateRussianTiles();
    }

    private void GenerateEnglishTiles()
    {
        foreach (char letter in EnglishLettersCount.Keys)
        {
            for (int i = 0; i < EnglishLettersCount[letter]; ++i)
            {
                Bag.Add(new Tile(Count, letter));
                ++Count;
            }
        }
    }

    private void GenerateRussianTiles()
    {
        foreach (char letter in RussianLettersCount.Keys)
        {
            for (int i = 0; i < RussianLettersCount[letter]; ++i)
            {
                Bag.Add(new Tile(Count, letter));
                ++Count;
            }
        }
    }

    public HashSet<Tile> GiveOutTiles(int count)
    {
        if (Bag.Count < count) 
            return [];

        var outTiles = new HashSet<Tile>();
        var bagList = Bag.ToList();

        for (int i = 0; i < count; ++i)
        {
            int randomIndex = _random.Next(0, bagList.Count);
            outTiles.Add(bagList[randomIndex]);
            Bag.Remove(bagList[randomIndex]);
            bagList.RemoveAt(randomIndex);
            --Count;
        }
        return outTiles;
    }

    public int RemainingTilesCount => Bag.Count;
    
    // Для будущего использования - переключение языка
    public void SwitchToRussian()
    {
        Bag.Clear();
        Count = 0;
        GenerateRussianTiles();
    }
    
    public void SwitchToEnglish()
    {
        Bag.Clear();
        Count = 0;
        GenerateEnglishTiles();
    }
}