using TagCloud;

namespace TagCloud2.Drawer;

public interface ICloudDrawer
{
    public Result<None> DrawTagsCloudFromFile(string filepath);
}