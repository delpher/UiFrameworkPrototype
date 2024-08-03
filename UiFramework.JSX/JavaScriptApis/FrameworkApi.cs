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
            Func<IDictionary<string, object?>?, ViewModelFactory[], ViewModelFactory> primitive =>
                ElementFactoryFromPrimitive(primitive, adaptedProps, adaptedChildren),
            ScriptObject jsComponent => ElementFactoryFromJsComponent(jsComponent, adaptedProps, adaptedChildren),
            _ => throw new InvalidOperationException("Failed to convert element to Element delegate")
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
        Framework.CreateElement(
            (p, c) =>
                (ElementFactory)jsComponent.InvokeAsFunction(p, c), adaptedProps, adaptedChildren);

    private static ElementFactory ElementFactoryFromPrimitive(
        Func<IDictionary<string, object?>?, ViewModelFactory[], ViewModelFactory> primitive,
        IDictionary<string, object?> adaptedProps, ElementFactory[] adaptedChildren) =>
        Framework.CreateElement(new Primitive(primitive), adaptedProps, adaptedChildren);

    private static ElementFactory ElementFactoryFromComponent(
        Func<IDictionary<string, object?>?, ElementFactory?[]?, ElementFactory> component,
        IDictionary<string, object?> adaptedProps, ElementFactory[] adaptedChildren) =>
        Framework.CreateElement(new(component), adaptedProps, adaptedChildren);

    public object?[] useState(object initialState)
    {
        var (state, setState) = UseState(initialState);
        return [state, setState];
    }
}
