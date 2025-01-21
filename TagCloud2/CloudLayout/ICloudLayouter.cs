using System.Drawing;

namespace TagCloud2.CloudLayout;

public interface ICloudLayouter
{
    public Rectangle PutNextRectangle(Size rectangleSize);
}