using TagCloud2;

namespace TagCloud.TextPreparator;

public interface IWordsFrequency
{
    public Result<IDictionary<string, int>> GetWordsFrequencyFromFile(string fileName);
}