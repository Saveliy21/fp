﻿using System.Drawing;
using TagCloud;
using TagCloud2.CloudLayout;
using TagCloud2.Drawer;

namespace TagCloud2.CloudGenerator;

public class RectanglesGenerator(ICloudLayouter cloudLayouter, DrawerSettings drawerSettings) : IRectanglesGenerator
{
    private readonly List<WordInShape> _wordsInShape = new();
    private const int MinRectangleWidth = 5;
    private const int MinRectangleHeight = 5;


    public Result<IList<WordInShape>> GetWordsInShape(IDictionary<string, int> wordToWeight)
    {
        foreach (var word in wordToWeight)
        {
            var current = word.Key;
            var size = GenerateRectangleSize(word, wordToWeight.Count);
            var rectangle = cloudLayouter.PutNextRectangle(size);
            if (rectangle.Width / current.Length < 1)
            {
                return Result.Fail<IList<WordInShape>>("Too small cloud size, try to take greater parameter");
            }
            
            var fontSize = GenerateFontSize(rectangle, current);
            _wordsInShape.Add(new WordInShape(current, rectangle, fontSize));
        }

        return _wordsInShape;
    }

    private float GenerateFontSize(Rectangle rectangle, string word)
    {
        var fontSizeWidth = rectangle.Width / word.Length;
        var fontSizeHeight = rectangle.Height;
        return Math.Min(fontSizeWidth, fontSizeHeight);
    }

    private Size GenerateRectangleSize(KeyValuePair<string, int> word, int count)
    {
        var length = word.Key.Length;
        var value = word.Value;
        var width = length * value * drawerSettings.CloudSize.Width / count;
        var height = value * drawerSettings.CloudSize.Height / count;
        return new Size(Math.Max(width, MinRectangleWidth),
            Math.Max(height, MinRectangleHeight));
    }
}