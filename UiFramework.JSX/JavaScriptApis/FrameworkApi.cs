using System.Diagnostics.CodeAnalysis;
using Microsoft.ClearScript;
using static UiFramework.Framework;

namespace UiFramework.JSX.JavaScriptApis;

// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global
// ReSharper disable MemberCanBePrivate.Global
[SuppressMessage("Performance", "CA1822:Mark members as static")]
public class FrameworkApi
{
    public ElementFactory createElement(object element, object props, object children)
    {
        var adaptedProps = (IDictionary<string, object?>)props;
        var adaptedChildren = AdaptChildren(children);

        return element switch
        {
            Func<IDictionary<string, object?>?, ElementFactory?[]?, ElementFactory> component =>
                ElementFactoryFromComponent(component, adaptedProps, adaptedChildren),
            Component component => CreateElement(component, adaptedProps, adaptedChildren),
            Func<IDictionary<string, object?>?, ViewModelFactory[], ViewModelFactory> primitive =>
                ElementFactoryFromPrimitive(primitive, adaptedProps, adaptedChildren),
            ScriptObject jsComponent => ElementFactoryFromJsComponent(jsComponent, adaptedProps, adaptedChildren),
            not null when element as Primitive == Framework.Fragment => ElementFromFragment(adaptedProps, adaptedChildren),
            _ => throw new InvalidOperationException($"Failed to convert element of type {element?.GetType().Name} to Element delegate")
        };
    }

    private static ElementFactory[] AdaptChildren(object children) =>
        (children as IList<object> ?? [])
        .SelectMany(child => child switch
        {
            ElementFactory e => [e],
            IList<object> elements => elements.Cast<ElementFactory>(),
            _ => [default]
        }).ToArray();

    private static ElementFactory ElementFactoryFromJsComponent(
        ScriptObject jsComponent,
        IDictionary<string, object?> adaptedProps,
        ElementFactory[] adaptedChildren) =>
        CreateElement(
            (p, c) =>
                (ElementFactory)jsComponent.InvokeAsFunction(p, c), adaptedProps, adaptedChildren);

    private static ElementFactory ElementFactoryFromPrimitive(
        Func<IDictionary<string, object?>?, ViewModelFactory[], ViewModelFactory> primitive,
        IDictionary<string, object?> adaptedProps, ElementFactory[] adaptedChildren) =>
        CreateElement(new Primitive(primitive), adaptedProps, adaptedChildren);

    private static ElementFactory ElementFactoryFromComponent(
        Func<IDictionary<string, object?>?, ElementFactory?[]?, ElementFactory> component,
        IDictionary<string, object?> adaptedProps, ElementFactory[] adaptedChildren) =>
        CreateElement(new(component), adaptedProps, adaptedChildren);

    private static ElementFactory ElementFromFragment(IDictionary<string, object?> props, ElementFactory[] children) =>
        CreateElement(Framework.Fragment, props, children);

    public object?[] useState(object initialState)
    {
        var (state, setState) = UseState(initialState);
        return [state, setState];
    }

    public void useEffect(object effect, object dependencies) =>
        UseEffect(() => ((dynamic)effect)(), (dependencies as IList<object>)?.ToArray() ?? []);

    public readonly object Fragment = Framework.Fragment;

    public Context createContext() => CreateContext();

    public object? useContext(Context context) => UseContext(context);
}
