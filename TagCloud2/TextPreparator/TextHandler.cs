using TagCloud2;
using TagCloud2.FileReader;

namespace TagCloud.TextPreparator;

public class TextHandler(ITextFilter textFilter, IFileReader fileReader) : IWordsFrequency
{
    private readonly Dictionary<string, int> _wordCount = new();

    private Result<IDictionary<string, int>> GetWordsFrequency(IEnumerable<string> words)
    {
        foreach (var word in words)
        {
            if (!_wordCount.TryAdd(word, 1))
                _wordCount[word]++;
        }

        return _wordCount;
    }

    public Result<IDictionary<string, int>> GetWordsFrequencyFromFile(string fileName)
    {
        return fileReader.TryReadFile(fileName)
            .Then(words => textFilter.GetFilteredText(words))
            .Then(filteredWords => GetWordsFrequency(filteredWords));
    }
}