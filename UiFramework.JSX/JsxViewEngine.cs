using System.Reflection;
using Microsoft.ClearScript.V8;
using UiFramework.Elements;
using UiFramework.JSX.JavaScriptApis;

namespace UiFramework.JSX;

public class JsxViewEngine(RootController rootController) : IDisposable
{
    private bool _initialized;

    private readonly V8ScriptEngine _jsEngine = new(
        // V8ScriptEngineFlags.EnableDebugging |
        // V8ScriptEngineFlags.EnableRemoteDebugging |
        // V8ScriptEngineFlags.AwaitDebuggerAndPauseOnStart
    );

    private readonly Babel _babel = new();

    public void Render(string jsx)
    {
        EnsureInitialized();

        var scriptSource = _babel.Transform(jsx);
        var compiledScript = _jsEngine.Compile(scriptSource);

        rootController.Render((ElementFactory)_jsEngine.Evaluate(compiledScript));
    }

    private void EnsureInitialized()
    {
        if (_initialized) return;
        AddFrameworkApi();
        ExposeComponents();
        _initialized = true;
    }

    private void AddFrameworkApi()
    {
        _jsEngine.AddHostObject("Framework", new FrameworkApi());
        _jsEngine.Execute(ScriptResources.Read("api-bridge.js"));
    }

    private void ExposeComponents()
    {
        _jsEngine.ExposeHostObjectStaticMembers = true;
        _jsEngine.AddHostObject("Components", new Components());
        foreach (var methodInfo in typeof(Components).GetMethods(BindingFlags.Static | BindingFlags.Public))
        {
            _jsEngine.AddHostObject(methodInfo.Name,
                methodInfo.CreateDelegate<Func<IDictionary<string, object?>, ViewModelFactory[], ViewModelFactory>>());
        }
    }

    public void Dispose()
    {
        _jsEngine.Dispose();
        _babel.Dispose();
        GC.SuppressFinalize(this);
    }

    public JsxViewEngine SetDebugOutput(Action<object> output)
    {
        _jsEngine.AddHostObject("DebugOutput", output);
        return this;
    }
}
