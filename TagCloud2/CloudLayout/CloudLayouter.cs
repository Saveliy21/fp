using System.Drawing;
using TagCloud2.CloudForms;

namespace TagCloud2.CloudLayout;

public class CloudLayouter(ICloudForm cloudForm) : ICloudLayouter
{
    public readonly List<Rectangle> Rectangles = new();

    public Rectangle PutNextRectangle(Size rectangleSize)
    {
        Rectangle rectangle;

        do
        {
            var point = cloudForm.GetNextPoint();
            point.Offset(-rectangleSize.Width / 2, -rectangleSize.Height / 2);
            rectangle = new Rectangle(point, rectangleSize);
        } while (Rectangles.Any(r => r.IntersectsWith(rectangle)));

        Rectangles.Add(rectangle);
        return rectangle;
    }
}