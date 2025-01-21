using FluentAssertions;
using NUnit.Framework;
using TagCloud.TextPreparator;
using TagCloud2.FileReader;
using static System.IO.File;

namespace TagCloud2Tests;

public class TextPreparatorTests
{
    private const string TempFileName = "temp.txt";
    private static readonly TxtReader TxtReader = new();
    private static readonly TextFilter TextFilter = new();
    private static readonly TextHandler TextHandler = new TextHandler(TextFilter, TxtReader);

    [TearDown]
    public void TearDown()
    {
        if (Exists(TempFileName))
        {
            Delete(TempFileName);
        }
    }

    [Test]
    public void TryReadFile_ShouldThrowException_WithNotExistedFile()
    {
        var invalidFilePath = "NotExistedfile.txt";

        var actual = TxtReader.TryReadFile(invalidFilePath);

        actual.IsSuccess.Should().BeFalse();
        actual.Error.Should().Be($"File not found: {invalidFilePath}");
    }

    [Test]
    public void TryReadFile_ShouldReduceToLowerCase_WhenReadingFile()
    {
        var words = new[]
        {
            "Рим",
            "Сава",
            "ШлЯпА"
        };
        WriteAllLines(TempFileName, words);
        var expected = new[]
        {
            "рим",
            "сава",
            "шляпа"
        };


        var text = TxtReader.TryReadFile(TempFileName);
        var actual = TextFilter.GetFilteredText(text.Value);

        actual.Value.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void GetFilteredText_ShouldIgnoreSpaces_WhenReadingFile()
    {
        var words = new[]
        {
            "   кот",
            "стены    ",
            "   грыз   ",
            "    ",
            ""
        };
        WriteAllLines(TempFileName, words);
        var expected = new[]
        {
            "кот",
            "стены",
            "грыз"
        };


        var text = TxtReader.TryReadFile(TempFileName);
        var actual = TextFilter.GetFilteredText(text.Value);

        actual.Value.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void GetFilteredText_ShouldRemove_WhenTextContainsBoringWords()
    {
        var words = new List<string> {"когда", "собака", "гуляет", "на"};
        var expected = new List<string> {"собака", "гуляет"};

        var actual = TextFilter.GetFilteredText(words);

        actual.IsSuccess.Should().BeTrue();
        actual.Value.ToList().Should().BeEquivalentTo(expected);
    }

    [Test]
    public void GetFilteredText_ShouldReturnSameText_WhenNoBoringWords()
    {
        var expected = new List<string> {"собака", "гуляет"};

        var actual = TextFilter.GetFilteredText(expected);


        actual.IsSuccess.Should().BeTrue();
        actual.Value.ToList().Should().BeEquivalentTo(expected);
    }

    [Test]
    public void HandleText_ShouldReturnCorrectWordFrenquency()
    {
        var words = new[] {"собака", "гуляет", "собака"};
        WriteAllLines(TempFileName, words);
        var expected = new Dictionary<string, int>
        {
            {"собака", 2},
            {"гуляет", 1}
        };

        var actual = TextHandler.GetWordsFrequencyFromFile(TempFileName);

        actual.IsSuccess.Should().BeTrue();
        actual.Value.Should().BeEquivalentTo(expected);
    }
}