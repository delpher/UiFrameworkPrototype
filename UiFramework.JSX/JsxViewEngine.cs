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
        AddReactBridgeScript();

        var script = _jsEngine.Compile(_babel.Transform(jsx));

        rootController.Render(() => ((ViewModelFactory)_jsEngine.Evaluate(script))());
    }

    private void AddReactBridgeScript()
    {
        _jsEngine.Execute(ScriptResources.Read("react-bridge.js"));
    }

    private void ExposeFrameworkJsApi()
    {
        _jsEngine.AddHostObject("Framework", new FrameworkJsxAdapter(rootController));
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
