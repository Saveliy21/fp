using System.Drawing;
using FakeItEasy;
using FluentAssertions;
using NUnit.Framework;
using TagCloud2.CloudGenerator;
using TagCloud2.CloudLayout;
using TagCloud2.ColoringAlgorithms;
using TagCloud2.Drawer;

namespace TagCloud2Tests;

public class CloudGeneratorTests
{
    private ICloudLayouter cloudLayouter;
    private IColorAlgorithm colorAlgorithm;
    private DrawerSettings drawerSettings;
    private RectanglesGenerator rectanglesGenerator;
    private static readonly Rectangle Rectangle = new(0, 0, 10, 10);

    [SetUp]
    public void Setup()
    {
        cloudLayouter = A.Fake<ICloudLayouter>();
        colorAlgorithm = A.Fake<IColorAlgorithm>();
        drawerSettings = new DrawerSettings(colorAlgorithm, new Size(100, 100), "Georgia");
        rectanglesGenerator = new RectanglesGenerator(cloudLayouter, drawerSettings);
    }

    [Test]
    public void GetWordsInShape_ShouldReturnCorrectResultSize()
    {
        A.CallTo(() => cloudLayouter.PutNextRectangle(new Size(200, 33)))
            .Returns(new Rectangle(0, 0, 50, 50));
        A.CallTo(() => cloudLayouter.PutNextRectangle(new Size(333, 66)))
            .Returns(new Rectangle(0, 0, 50, 50));
        A.CallTo(() => cloudLayouter.PutNextRectangle(new Size(700, 100)))
            .Returns(new Rectangle(0, 0, 50, 50));
        var dict = new Dictionary<string, int>
        {
            {"собака", 1},
            {"кошка", 2},
            {"попугай", 3}
        };
        var expected = 3;

        var actual = rectanglesGenerator.GetWordsInShape(dict);

        actual.IsSuccess.Should().BeTrue();
        actual.Value.Count.Should().Be(expected);
    }

    [Test]
    public void GetWordsInShape_ShouldReturnCorrectResult()
    {
        A.CallTo(() => cloudLayouter.PutNextRectangle(new Size(300, 50)))
            .Returns(Rectangle);
        A.CallTo(() => cloudLayouter.PutNextRectangle(new Size(500, 100)))
            .Returns(Rectangle);

        var words = new Dictionary<string, int>
        {
            {"собака", 1},
            {"кошка", 2},
        };
        var expected = new List<WordInShape>
        {
            new("собака", Rectangle, 1),
            new("кошка", Rectangle, 2),
        };

        var actual = rectanglesGenerator.GetWordsInShape(words);

        actual.IsSuccess.Should().BeTrue();
        actual.Value.Should().BeEquivalentTo(expected);
    }
}