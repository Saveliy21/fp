using System.Drawing;
using TagCloud;

namespace TagCloud2.ColoringAlgorithms;

public interface IColorAlgorithm
{
    public Result<Color[]> GetColors(int count);
}