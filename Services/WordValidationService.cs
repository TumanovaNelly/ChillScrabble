using System.Text.Json;
using System.Reflection;
using ChillScrabble.Models;

namespace ChillScrabble.Services;

public class WordValidationService
{
    private readonly Dictionary<string, HashSet<string>> _englishWords = new();
    private readonly Dictionary<string, HashSet<string>> _russianWords = new();

    public WordValidationService()
    {
        LoadDictionary("english_words.json", _englishWords);
        LoadDictionary("russian_words.json", _russianWords);
    }

    private void LoadDictionary(string fileName, Dictionary<string, HashSet<string>> target)
    {
        try
        {
            var path = Path.Combine(
                Path.GetDirectoryName(Assembly.GetEntryAssembly()!.Location)!,
                "Dictionaries",
                fileName
            );

            if (!File.Exists(path))
                throw new FileNotFoundException($"File not found: {path}");

            var json = File.ReadAllText(path);

            // Попытка десериализации как массива
            try
            {
                var wordsArray = JsonSerializer.Deserialize<List<string>>(json);
                ProcessWords(wordsArray, fileName, target);
                return;
            }
            catch (JsonException) { }

            // Попытка десериализации как объекта с ключом "words"
            try
            {
                var wordsObject = JsonSerializer.Deserialize<Dictionary<string, List<string>>>(json);
                if (wordsObject != null && wordsObject.ContainsKey("words"))
                {
                    ProcessWords(wordsObject["words"], fileName, target);
                    return;
                }
            }
            catch (JsonException) { }

            throw new JsonException("Unsupported JSON format");
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException(
                $"Failed to load dictionary '{fileName}': {ex.Message}", 
                ex
            );
        }
    }

    private void ProcessWords(List<string>? words, string fileName, Dictionary<string, HashSet<string>> target)
    {
        if (words == null) return;

        bool isEnglish = fileName.Contains("english", StringComparison.OrdinalIgnoreCase);

        foreach (var word in words)
        {
            var key = GetLengthKey(word.Length, isEnglish);
            if (!target.ContainsKey(key)) target[key] = new HashSet<string>();
            target[key].Add(word.ToUpper());
        }
    }

    public bool ValidateWord(string word, bool isEnglish)
    {
        if (string.IsNullOrWhiteSpace(word)) return false;

        var lengthKey = GetLengthKey(word.Length, isEnglish);
        var dictionary = isEnglish ? _englishWords : _russianWords;

        return dictionary.TryGetValue(lengthKey, out var words) && 
              words.Contains(word.ToUpper());
    }

    private static string GetLengthKey(int length, bool isEnglish)
    {
        return length switch
        {
            2 => "two_letters",
            3 => "three_letters",
            4 => "four_letters",
            5 => "five_letters",
            6 => "six_letters",
            7 => "seven_letters",
            _ => "other"
        } + (isEnglish ? "_eng" : "_rus");
    }

    public static bool DetermineLanguage(string word)
    {
        return word.All(c => ChipsBag.EnglishLettersCount.ContainsKey(char.ToUpper(c)));
    }
}