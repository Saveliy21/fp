using TagCloud2;

namespace TagCloud.TextPreparator;

public interface ITextFilter
{
    public Result<IEnumerable<string>> GetFilteredText(IEnumerable<string> words);
}