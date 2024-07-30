using System.Diagnostics.CodeAnalysis;
using Microsoft.ClearScript;

namespace UiFramework.JSX;

// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global
// ReSharper disable MemberCanBePrivate.Global
[SuppressMessage("Performance", "CA1822:Mark members as static")]
public class FrameworkApi(RootController rootController)
{
    private readonly ElementFactory _elementFactory = new(rootController);

    public ViewModelFactory createElement(object element, object props, object children)
    {
        ElementDefinition? adaptedElement = null;

        if (element is Func<IDictionary<string, object?>, ViewModelFactory[], ViewModelFactory> elementDefinition)
            adaptedElement = new(elementDefinition);

        if (element is ScriptObject)
            adaptedElement = (p, c) => ((dynamic)element)(p, c);

        if (adaptedElement == null)
            throw new InvalidOperationException("Failed to convert element to ElementDefinition");

        var adaptedProps = (IDictionary<string, object?>)props;

        var adaptedChildren = (children as IList<object> ?? [])
            .SelectMany(child => child switch
            {
                ViewModelFactory viewModelFactory => [viewModelFactory],
                IList<object> list => list.Cast<ViewModelFactory>(),
                _ => []
            }).ToArray();

        return _elementFactory.CreateElement(adaptedElement, adaptedProps, adaptedChildren);
    }

    public object[] useState(object initialState)
    {
        var (state, setState) = StateManager.UseState(initialState);
        return [state, setState];
    }
}
