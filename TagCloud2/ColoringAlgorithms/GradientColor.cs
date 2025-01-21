using System.Drawing;
using TagCloud;

namespace TagCloud2.ColoringAlgorithms;

public class GradientColor(Color gradientFrom, Color gradientTo) : IColorAlgorithm
{
    public Result<Color[]> GetColors(int count)
    {
        if (!gradientFrom.IsKnownColor || !gradientTo.IsKnownColor)
        {
            return Result.Fail<Color[]>("Gradient colors are unknown");
        }

        return Enumerable.Range(0, count)
            .Select(i => Color.FromArgb(
                (int) (gradientFrom.R + (gradientTo.R - gradientFrom.R) * (float) i / (count - 1)),
                (int) (gradientFrom.G + (gradientTo.G - gradientFrom.G) * (float) i / (count - 1)),
                (int) (gradientFrom.B + (gradientTo.B - gradientFrom.B) * (float) i / (count - 1))
            ))
            .ToArray();
    }
}