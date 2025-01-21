using TagCloud;

namespace TagCloud2.FileReader;

public interface IFileReader
{
    public Result<IEnumerable<string>> TryReadFile(string filePath);
}