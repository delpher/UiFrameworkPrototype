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
    public Element? createElement(object element, object props, object children)
    {
        var adaptedProps = (IDictionary<string, object?>)props;
        var adaptedChildren = AdaptChildren(children);

        return element switch
        {
            Func<IDictionary<string, object?>?, Element?[]?, Element> component =>
                ElementFactoryFromComponent(component, adaptedProps, adaptedChildren),
            Component component => CreateElement(component, adaptedProps, adaptedChildren),
            Func<IDictionary<string, object?>?, ViewFactory[], ViewFactory> primitive =>
                ElementFactoryFromPrimitive(primitive, adaptedProps, adaptedChildren),
            ScriptObject jsComponent => ElementFactoryFromJsComponent(jsComponent, adaptedProps, adaptedChildren),
            not null when element as Primitive == Framework.Fragment => ElementFromFragment(adaptedProps, adaptedChildren),
            _ => throw new InvalidOperationException($"Failed to convert element of type {element?.GetType().Name} to Element delegate")
        };
    }

    private static Element[] AdaptChildren(object children) =>
        (children as IList<object> ?? [])
        .SelectMany(child => child switch
        {
            Element e => [e],
            IList<object> elements => elements.Cast<Element>(),
            _ => [default]
        }).ToArray();

    private static Element ElementFactoryFromJsComponent(
        ScriptObject jsComponent,
        IDictionary<string, object?> adaptedProps,
        Element[] adaptedChildren) =>
        CreateElement(
            (p, c) =>
                (Element)jsComponent.InvokeAsFunction(p, c), adaptedProps, adaptedChildren);

    private static Element ElementFactoryFromPrimitive(
        Func<IDictionary<string, object?>?, ViewFactory[], ViewFactory> primitive,
        IDictionary<string, object?> adaptedProps, Element[] adaptedChildren) =>
        CreateElement(new Primitive(primitive), adaptedProps, adaptedChildren);

    private static Element ElementFactoryFromComponent(
        Func<IDictionary<string, object?>?, Element?[]?, Element> component,
        IDictionary<string, object?> adaptedProps, Element[] adaptedChildren) =>
        CreateElement(new Component(component), adaptedProps, adaptedChildren);

    private static Element ElementFromFragment(IDictionary<string, object?> props, Element[] children) =>
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
