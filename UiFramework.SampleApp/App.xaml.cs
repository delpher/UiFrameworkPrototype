using System.IO;
using System.Reflection;

namespace UiFramework.SampleApp;

public partial class App
{
    public static string ReadResource(string resourcePath)
    {
        var assembly = Assembly.GetExecutingAssembly().GetName();
        var s = System.IO.Packaging.PackUriHelper.UriSchemePack;
        var resourceUri = new Uri($"pack://application:,,,/{assembly};component//{resourcePath}");
        var resourceStream = GetResourceStream(resourceUri);
        using var streamReader = new StreamReader(resourceStream!.Stream);
        return streamReader.ReadToEnd();
    }
}
