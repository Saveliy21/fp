using TagCloud;

namespace TagCloud2.CloudGenerator;

public interface IRectanglesGenerator
{
    public Result<IList<WordInShape>> GetWordsInShape(IDictionary<string, int> wordToWeight);
}