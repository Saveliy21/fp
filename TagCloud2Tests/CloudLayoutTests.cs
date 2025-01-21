using System.Drawing;
using FluentAssertions;
using NUnit.Framework;
using TagCloud2;
using TagCloud2.CloudForms;
using TagCloud2.CloudLayout;

namespace TagCloud2Tests;

public class CloudLayoutTests
{
    private static CloudLayouter _cloudLayouter;
    private static ICloudForm _cloudForm;


    [SetUp]
    public void SetUp()
    {
        _cloudForm = new CircularSpiral(new Point(500, 500));
        _cloudLayouter = new CloudLayouter(_cloudForm);
    }

    [Test]
    public void PutNextRectangle_ShouldAddNewRectangle()
    {
        var size = new Size(10, 20);
        var coordinateX = 500 - size.Width / 2;
        var coordinateY = 500 - size.Height / 2;
        var expected = new Rectangle(coordinateX, coordinateY, size.Width, size.Height);

        _cloudLayouter.PutNextRectangle(size);

        _cloudLayouter.Rectangles.Should().Contain(expected);
    }

    [Test]
    public void CloudLayouter_RectAngelsListShouldHaveCorrectSize()
    {
        _cloudLayouter.PutNextRectangle(new Size(40, 20));
        _cloudLayouter.PutNextRectangle(new Size(20, 40));
        _cloudLayouter.PutNextRectangle(new Size(50, 50));

        _cloudLayouter.Rectangles.Count.Should().Be(3);
    }

    [Test]
    public void CloudLayouter_RectAngelsShouldNoIntersectsWithOthers()
    {
        Random rnd = new Random();
        for (int i = 0; i < 100; i++)
        {
            int width = rnd.Next(15, 40);
            int height = rnd.Next(15, 40);
            _cloudLayouter.PutNextRectangle(new Size(width, height));
        }

        List<Rectangle> rectangels = _cloudLayouter.Rectangles;

        foreach (Rectangle rectangle in rectangels)
        {
            rectangels.Where((_, j) => j != rectangels.IndexOf(rectangle))
                .All(r => !r.IntersectsWith(rectangle))
                .Should().BeTrue();
        }
    }

    [Test]
    public void GetNextPoint_ShouldReturnPointsOnSpiral_WithCircularSpiral()
    {
        var spiral = new CircularSpiral(new Point(500, 500));
        var expected = new Point[]
        {
            new(500, 500), new(500, 500), new(499, 501), new(497, 500), new(497, 496),
            new(501, 495), new(505, 498)
        };

        var actual = new List<Point>();
        for (int i = 0; i < 700; i++)
        {
            var point = spiral.GetNextPoint();
            if (i % 100 == 0)
                actual.Add(point);
        }

        actual.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void GetNextPoint_ShouldReturnPointsOnSpiral_WithSquareSpiral()
    {
        var spiral = new SquareSpiral(new Point(500, 500));
        var expected = new Point[]
        {
            new(500, 501), new(501, 501), new(501, 500),
            new(501, 499), new(500, 499), new(499, 499), new(499, 500),
        };

        var actual = new List<Point>();
        for (int i = 0; i < 7; i++)
        {
            var point = spiral.GetNextPoint();
            actual.Add(point);
        }

        actual.Should().BeEquivalentTo(expected);
    }


    [Test, MaxTime(5000)]
    public void CloudLayouter_ShouldWorkInTime()
    {
        Random rnd = new Random();
        for (int i = 0; i < 10000; i++)
        {
            int width = rnd.Next(1, 10);
            int height = rnd.Next(1, 10);
            _cloudLayouter.PutNextRectangle(new Size(width, height));
        }
    }
}