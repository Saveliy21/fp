using TagCloud;

namespace TagCloud2.FileReader;

public class TxtReader : IFileReader
{
    public Result<IEnumerable<string>> TryReadFile(string filePath)
    {
        try
        {
            File.ReadAllText(filePath);
        }
        catch (FileNotFoundException)
        {
            return Result.Fail<IEnumerable<string>>($"File not found: {filePath}");
        }
        catch (UnauthorizedAccessException)
        {
            return Result.Fail<IEnumerable<string>>($"Access to the file is denied: {filePath}");
        }
        catch (IOException ex)
        {
            return Result.Fail<IEnumerable<string>>($"Error reading file: {ex.Message}");
        }

        return Result.Ok(File.ReadLines(filePath));
    }
}