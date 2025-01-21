using System.Drawing;
using TagCloud2.ColoringAlgorithms;

namespace TagCloud2.Drawer;

public record DrawerSettings(IColorAlgorithm WordsColor, Size CloudSize, string Font);