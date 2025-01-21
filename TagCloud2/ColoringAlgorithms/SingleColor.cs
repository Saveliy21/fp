using System.Drawing;
using TagCloud;

namespace TagCloud2.ColoringAlgorithms;

public class SingleColor(Color color) : IColorAlgorithm
{
    public Result<Color[]> GetColors(int count)
    {
        return !color.IsKnownColor
            ? Result.Fail<Color[]>("Unknown color")
            : Enumerable.Repeat(color, count).ToArray();
    }
}