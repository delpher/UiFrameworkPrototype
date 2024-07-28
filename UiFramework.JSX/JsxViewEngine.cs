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
        var transpiledViewScript = _babel.Transform(jsx);
        _jsEngine.Execute("""
                          const React = { createElement: (element, props, ...children) => { 
                                if (Elements.Exposes(element)) 
                                    return Framework.createElement(element, props, children);
                                return element.bind(Framework.createContext)({...props, children});
                            }}
                          
                          const useState = Framework.useState;
                          """);
        _jsEngine.ExposeHostObjectStaticMembers = true;
        _jsEngine.AddHostObject("Framework", new FrameworkJsxAdapter(rootController));

        _jsEngine.AddHostObject("Elements", new Elements.Elements());
        foreach (var methodInfo in typeof(Elements.Elements).GetMethods(BindingFlags.Static | BindingFlags.Public))
        {
            _jsEngine.AddHostObject(methodInfo.Name,
                methodInfo.CreateDelegate<Func<IDictionary<string, object?>, ViewModelFactory[], ViewModelFactory>>());
        }

        rootController.Render((ViewModelFactory)_jsEngine.Evaluate(transpiledViewScript));
    }

    public void Dispose()
    {
        _jsEngine.Dispose();
        _babel.Dispose();
        GC.SuppressFinalize(this);
    }
}
