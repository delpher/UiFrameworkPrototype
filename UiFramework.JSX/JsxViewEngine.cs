using System.Reflection;
using Microsoft.ClearScript.V8;

namespace UiFramework.JSX;

public class JsxViewEngine(RootController rootController) : IDisposable
{
    private readonly V8ScriptEngine _jsEngine = new(
        // V8ScriptEngineFlags.EnableDebugging |
        // V8ScriptEngineFlags.EnableRemoteDebugging |
        // V8ScriptEngineFlags.AwaitDebuggerAndPauseOnStart
    );

    private readonly Babel _babel = new();

    public void Render(string jsx)
    {
        ExposeFrameworkJsApi();
        ExposeUiElements();
        AddFrameworkApi();

        var scriptSource = _babel.Transform(jsx);
        var script = _jsEngine.Compile(scriptSource);

        rootController.Render(() => ((ViewModelFactory)_jsEngine.Evaluate(script))());
    }

    private void AddFrameworkApi()
    {
        _jsEngine.Execute(ScriptResources.Read("api-bridge.js"));
    }

    private void ExposeFrameworkJsApi()
    {
        _jsEngine.AddHostObject("Framework", new FrameworkApi(rootController));
    }

    private void ExposeUiElements()
    {
        _jsEngine.ExposeHostObjectStaticMembers = true;
        _jsEngine.AddHostObject("Elements", new Elements.Elements());
        foreach (var methodInfo in typeof(Elements.Elements).GetMethods(BindingFlags.Static | BindingFlags.Public))
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
}
