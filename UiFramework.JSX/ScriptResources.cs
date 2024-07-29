using System.Reflection;

namespace UiFramework.JSX;

public static class ScriptResources
{
    public static string Read(string name)
    {
        var assembly = Assembly.GetAssembly(typeof(ScriptResources));
        var resourceName = $"{assembly!.GetName().Name}.{name}";
        using var stream = assembly.GetManifestResourceStream(resourceName);
        using var reader = new StreamReader(stream!);
        return reader.ReadToEnd();
    }

}