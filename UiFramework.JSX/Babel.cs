using System.Reflection;
using Microsoft.ClearScript.V8;

namespace UiFramework.JSX;

public class Babel : IDisposable
{
    private readonly V8ScriptEngine _engine = new();

    public string Transform(string sourceJs)
    {
        var babelScript = ReadBabelScriptFromResource();
        _engine.Execute(babelScript);
        _engine.AddHostObject("ScriptSource", new { script = sourceJs });
        return (string)_engine.Evaluate("Babel.transform(ScriptSource.script, {presets: ['react','env']}).code");
    }

    private string ReadBabelScriptFromResource()
    {
        var assembly = Assembly.GetAssembly(GetType());
        var resourceName = $"{assembly!.GetName().Name}.babel.min.js";
        using var stream = assembly.GetManifestResourceStream(resourceName);
        using var reader = new StreamReader(stream!);
        return reader.ReadToEnd();
    }

    public void Dispose()
    {
        _engine.Dispose();
        GC.SuppressFinalize(this);
    }
}
