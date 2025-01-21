using System.Drawing;

namespace TagCloud2;

public static class SaviorImages
{
    public static void SaveImage(Bitmap bmp, string fileName, string imageFormat)
    {
        fileName = $"{fileName}.{imageFormat.ToLower()}";
        bmp.Save(fileName);
    }
}