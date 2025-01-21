using System.Drawing;
using Autofac;
using CommandLine;
using TagCloud.TextPreparator;
using TagCloud2.CloudForms;
using TagCloud2.CloudGenerator;
using TagCloud2.CloudLayout;
using TagCloud2.ColoringAlgorithms;
using TagCloud2.Drawer;
using TagCloud2.FileReader;

namespace TagCloud2.TagCloudApp;

public static class Program
{
    public static void Main(string[] args)
    {
        try
        {
            Parser.Default.ParseArguments<Options>(args)
                .WithParsed(options =>
                {
                    var container = ConfigureContainer(options);
                    container.Resolve<IWordsFrequency>();
                    var generator = container.Resolve<ICloudDrawer>();
                    var result = generator.DrawTagsCloudFromFile(options.FilePath);
                    Console.WriteLine(result.IsSuccess
                        ? $"Путь к файлу: {options.FilePath} " + Environment.NewLine +
                          $"Алгоритм: {options.TagCloudAlgorithm}" + Environment.NewLine +
                          $"Алгоритм расцветки слов: {options.WordsColor}" + Environment.NewLine +
                          $"Шрифт: {options.Font}" + Environment.NewLine +
                          $"Размер: {options.Size}"
                        : $"Failed to save image : {result.Error}");
                })
                .WithNotParsed(errors => { Console.WriteLine("Error in command line parameters."); });
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    private static IContainer ConfigureContainer(Options options)
    {
        var builder = new ContainerBuilder();
        builder.RegisterType<TxtReader>().As<IFileReader>();
        builder.RegisterType<TextFilter>().As<ITextFilter>();
        builder.RegisterType<TextHandler>().As<IWordsFrequency>();
        builder.Register(BuildColoringAlgorithm(options));
        builder.Register(BuildDrawerSettings(options));
        builder.Register(BuildCloudForm(options));
        builder.RegisterType<CloudLayouter>().As<ICloudLayouter>();
        builder.RegisterType<RectanglesGenerator>().As<IRectanglesGenerator>();
        builder.RegisterType<RectanglesCloudDrawer>().As<ICloudDrawer>();
        return builder.Build();
    }
    
    private static Func<IComponentContext, IColorAlgorithm> BuildColoringAlgorithm(Options options)
    {
        if (options.WordsColor.ToLower().Equals("random"))
        {
            return c => new RandomColor();
        }

        var gradient = options.WordsColor.Split("-").Select(Color.FromName).ToArray();
        if (gradient.Length == 1)
        {
            return c => new SingleColor(Color.FromName(options.WordsColor));
        }

        return c => new GradientColor(gradient[0], gradient[1]);
    }

    private static Func<IComponentContext, DrawerSettings> BuildDrawerSettings(Options options) =>
        c => new DrawerSettings(
            c.Resolve<IColorAlgorithm>(),
            new Size(options.Size, options.Size),
            options.Font);

    private static Func<IComponentContext, ICloudForm> BuildCloudForm(Options options)
    {
        return options.TagCloudAlgorithm switch
        {
            TagCloudAlgorithms.Circular => _ => new CircularSpiral(new Point(options.Size / 2, options.Size / 2)),
            TagCloudAlgorithms.Square => _ => new SquareSpiral(new Point(options.Size / 2, options.Size / 2)),
            _ => _ => new CircularSpiral(new Point(options.Size / 2, options.Size / 2))
        };
    }
}