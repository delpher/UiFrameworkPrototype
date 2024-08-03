using Microsoft.ClearScript.V8;

namespace UiFramework.JSX;

public class Babel : IDisposable
{
    private readonly V8ScriptEngine _engine = new();

    public string Transform(string sourceJs)
    {
        _engine.Execute(ScriptResources.Read("babel.min.js"));
        _engine.AddHostObject("ScriptSource", new { script = sourceJs });
        return (string)_engine.Evaluate("Babel.transform(ScriptSource.script, {presets: ['react','env']}).code");
    }

    public void Dispose()
    {
        _engine.Dispose();
        GC.SuppressFinalize(this);
    }
}
