using System.IO;
using System.Reflection;
using System.Text.Json;
using ChillScrabble.Services;
using Xunit;

namespace ChillScrabble.Tests.Services;

public class WordValidationServiceTests : IDisposable
{
    private readonly string _dictionariesPath;

    public WordValidationServiceTests()
    {
        var directory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!;
        _dictionariesPath = Path.Combine(directory, "Dictionaries");
        Directory.CreateDirectory(_dictionariesPath);

        // Create test English dictionary
        var englishWords = new List<string> { "CAT", "TEST", "EIGHTEEN" };
        File.WriteAllText(
            Path.Combine(_dictionariesPath, "english_words.json"),
            JsonSerializer.Serialize(englishWords)
        );

        // Create test Russian dictionary
        var russianWords = new List<string> { "КОТ", "ТЕСТ", "ВОСЕМЬДЕСЯТ" };
        File.WriteAllText(
            Path.Combine(_dictionariesPath, "russian_words.json"),
            JsonSerializer.Serialize(russianWords)
        );
    }

    public void Dispose()
    {
        Directory.Delete(_dictionariesPath, true);
    }

    [Fact]
    public void ValidateWord_EnglishWordExists_ReturnsTrue()
    {
        var service = new WordValidationService();
        Assert.True(service.ValidateWord("CAT", true));
    }

    [Fact]
    public void ValidateWord_EnglishWordNotExists_ReturnsFalse()
    {
        var service = new WordValidationService();
        Assert.False(service.ValidateWord("DOG", true));
    }

    [Fact]
    public void ValidateWord_RussianWordExists_ReturnsTrue()
    {
        var service = new WordValidationService();
        Assert.True(service.ValidateWord("КОТ", false));
    }

    [Fact]
    public void ValidateWord_RussianWordNotExists_ReturnsFalse()
    {
        var service = new WordValidationService();
        Assert.False(service.ValidateWord("СОБАКА", false));
    }

    [Fact]
    public void ValidateWord_OtherLengthCategory_ReturnsTrue()
    {
        var service = new WordValidationService();
        Assert.True(service.ValidateWord("EIGHTEEN", true));
    }

    [Fact]
    public void ValidateWord_CaseInsensitive_ReturnsTrue()
    {
        var service = new WordValidationService();
        Assert.True(service.ValidateWord("cat", true));
        Assert.True(service.ValidateWord("CaT", true));
    }

    [Fact]
    public void ValidateWord_EmptyString_ReturnsFalse()
    {
        var service = new WordValidationService();
        Assert.False(service.ValidateWord("", true));
    }

    [Fact]
    public void ValidateWord_Null_ReturnsFalse()
    {
        var service = new WordValidationService();
        Assert.False(service.ValidateWord(null!, true));
    }

    [Fact]
    public void DetermineLanguage_AllEnglishLetters_ReturnsTrue()
    {
        Assert.True(WordValidationService.DetermineLanguage("Test"));
    }

    [Fact]
    public void DetermineLanguage_WithRussianLetters_ReturnsFalse()
    {
        Assert.False(WordValidationService.DetermineLanguage("Привет"));
    }

    [Fact]
    public void DetermineLanguage_EmptyString_ReturnsTrue()
    {
        Assert.True(WordValidationService.DetermineLanguage(""));
    }

    [Fact]
    public void DetermineLanguage_WordWithNumber_ReturnsFalse()
    {
        Assert.False(WordValidationService.DetermineLanguage("HELL0"));
    }

    [Fact]
    public void ValidateWord_RussianCaseInsensitive_ReturnsTrue()
    {
        var service = new WordValidationService();
        Assert.True(service.ValidateWord("кот", false));
    }

    [Fact]
    public void DetermineLanguage_MixedLetters_ReturnsFalse()
    {
        Assert.False(WordValidationService.DetermineLanguage("CТОP"));
    }
}