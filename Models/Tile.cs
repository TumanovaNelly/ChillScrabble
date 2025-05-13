using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace ChillScrabble.Models;

public class Tile(int id, char letter)
{
    public char Letter { get; } = letter;
    public int Value { get; } = LettersValues[letter];
    public int Id { get; } = id;

    [JsonIgnore]
    private static readonly IReadOnlyDictionary<char, int> LettersValues = new ReadOnlyDictionary<char, int>(
        new Dictionary<char, int>
        {
            // Русские буквы
            ['А'] = 1, ['В'] = 1, ['Е'] = 1, ['И'] = 1, ['Н'] = 1, ['О'] = 1, ['Р'] = 1, ['С'] = 1, ['Т'] = 1,
            ['Д'] = 2, ['К'] = 2, ['Л'] = 2, ['М'] = 2, ['П'] = 2, ['У'] = 2,
            ['Б'] = 3, ['Г'] = 3, ['Ь'] = 3, ['Я'] = 3,
            ['Й'] = 4, ['Ы'] = 4,
            ['Ж'] = 5, ['З'] = 5, ['Х'] = 5, ['Ц'] = 5, ['Ч'] = 5,
            ['Ф'] = 8, ['Ш'] = 8, ['Э'] = 8, ['Ю'] = 8,
            ['Щ'] = 10,
            ['Ъ'] = 15,

            // Английские буквы
            ['A'] = 1, ['E'] = 1, ['I'] = 1, ['L'] = 1, ['N'] = 1, ['O'] = 1, ['R'] = 1, ['S'] = 1, ['T'] = 1,
            ['U'] = 1,
            ['D'] = 2, ['G'] = 2,
            ['B'] = 3, ['C'] = 3, ['M'] = 3, ['P'] = 3,
            ['F'] = 4, ['H'] = 4, ['V'] = 4, ['W'] = 4, ['Y'] = 4,
            ['K'] = 5,
            ['J'] = 8, ['X'] = 8,
            ['Q'] = 10, ['Z'] = 10
        });
}