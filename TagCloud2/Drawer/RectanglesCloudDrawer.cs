using System.Drawing;
using TagCloud;
using TagCloud.TextPreparator;
using TagCloud2.CloudGenerator;
using TagCloud2.CloudLayout;
using Color = System.Drawing.Color;
using Font = System.Drawing.Font;
using Size = System.Drawing.Size;


namespace TagCloud2.Drawer;

public class RectanglesCloudDrawer(
    DrawerSettings drawerSettings,
    IWordsFrequency wordsFrequency,
    IRectanglesGenerator rectanglesGenerator)
    : ICloudDrawer
{
    private Result<Bitmap> DrawCloud(IList<WordInShape> words, Size size)
    {
        var bmp = new Bitmap(size.Width, size.Height);
        using var graphics = Graphics.FromImage(bmp);

        var bgColor = Color.White;
        graphics.Clear(bgColor);
        var colors = drawerSettings.WordsColor.GetColors(words.Count);
        var colorsValue = colors.Value;
        if (!colors.IsSuccess)
        {
            return Result.Fail<Bitmap>(colors.Error);
        }

        var rectBrush = new SolidBrush(bgColor);
        var i = 0;
        var fontFamilies = FontFamily.Families;
        var isFontExist = fontFamilies.Any(f =>
            f.Name.Equals(drawerSettings.Font, StringComparison.OrdinalIgnoreCase));
        if (!isFontExist)
        {
            return Result.Fail<Bitmap>("Current font doesn't exist");
        }

        foreach (var (word, rect, fontSize) in words)
        {
            var textBrush = new SolidBrush(colorsValue[i++]);
            graphics.FillRectangle(rectBrush, rect);
            var font = new Font(drawerSettings.Font, fontSize);

            var stringFormat = new StringFormat()
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };

            graphics.DrawString(word, font, textBrush, rect, stringFormat);
        }

        return bmp;
    }

    public Result<None> DrawTagsCloudFromFile(string filepath)
    {
        return wordsFrequency.GetWordsFrequencyFromFile(filepath)
            .Then(word => rectanglesGenerator.GetWordsInShape(word))
            .Then(wordsInShape => DrawCloud(wordsInShape, wordsInShape.GetCloudSize()))
            .Then(result => SaviorImages.SaveImage(result, "TagCloud", "PNG"));
    }
}