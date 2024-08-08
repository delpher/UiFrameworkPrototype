using System.IO;
using System.Reflection;

namespace UiFramework.JSX.SampleApp;

public partial class App
{
    public static string ReadResource(string resourcePath)
    {
        var assembly = Assembly.GetExecutingAssembly().GetName();
        var resourceUri = new Uri($"pack://application:,,,/{assembly};component//{resourcePath}");
        var resourceStream = GetResourceStream(resourceUri);
        using var streamReader = new StreamReader(resourceStream!.Stream);
        return streamReader.ReadToEnd();
    }
}
