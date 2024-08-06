using Microsoft.ClearScript.V8;

namespace UiFramework.JSX;

public class Babel : IDisposable
{
    private readonly V8ScriptEngine _engine = new();
    private bool _scriptInitialized;

    public string Transform(string sourceJs)
    {
        EnsureBabel();
        _engine.AddHostObject("ScriptSource", new { script = sourceJs });
        return (string)_engine.Evaluate("Babel.transform(ScriptSource.script, {presets: ['react','env']}).code");
    }

    private void EnsureBabel()
    {
        if (_scriptInitialized) return;
        _engine.Execute(ScriptResources.Read("babel.min.js"));
        _scriptInitialized = true;
    }

    public void Dispose()
    {
        _engine.Dispose();
        GC.SuppressFinalize(this);
    }
}
