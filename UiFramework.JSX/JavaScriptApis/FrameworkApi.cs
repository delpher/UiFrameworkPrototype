using System.Diagnostics.CodeAnalysis;
using Microsoft.ClearScript;
using static UiFramework.StateManager;

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

        var adaptedChildren = (children as IList<object> ?? [])
            .SelectMany(child => child switch
            {
                ElementFactory e => [e],
                IList<object> elements => elements.Cast<ElementFactory>(),
                _ => [default]
            }).ToArray();


        if (element is Func<IDictionary<string, object?>?, ElementFactory?[]?, ElementFactory> component)
            return Framework.CreateElement(new(component), adaptedProps, adaptedChildren);

        if (element is Func<IDictionary<string, object?>?, ViewModelFactory[], ViewModelFactory> primitive)
            return Framework.CreateElement(new Primitive(primitive), adaptedProps, adaptedChildren);

        if (element is ScriptObject jsComponent)
            return Framework.CreateElement(
                (p, c) => (ElementFactory)jsComponent.InvokeAsFunction(p, c), adaptedProps, adaptedChildren);

        throw new InvalidOperationException("Failed to convert element to Element delegate");
    }

    public object?[] useState(object initialState)
    {
        var (state, setState) = UseState(initialState);
        return [state, setState];
    }
}
