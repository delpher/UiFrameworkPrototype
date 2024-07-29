using System.Diagnostics.CodeAnalysis;

namespace UiFramework.JSX;

// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global
// ReSharper disable MemberCanBePrivate.Global
[SuppressMessage("Performance", "CA1822:Mark members as static")]
public class FrameworkApi(RootController rootController)
{
    private readonly ElementFactory _elementFactory = new(rootController);

    public void beginComponent() => rootController.MakeCurrent();

    public ViewModelFactory createElement(object element, object props, object children)
    {
        var adaptedChildren = (IEnumerable<object>?)children;
        return createElement((Func<IDictionary<string, object?>, ViewModelFactory[], ViewModelFactory>)element, props,
            adaptedChildren?.ToArray());
    }

    public ViewModelFactory createElement(
        Func<IDictionary<string, object?>, ViewModelFactory[], ViewModelFactory> element, object? props,
        object[]? children = null)
    {
        var adaptedProps = (IDictionary<string, object?>)(props ?? new Dictionary<string, object?>());

        var adaptedChildren = (children ?? [])
            .SelectMany(child => child switch
            {
                ViewModelFactory factory => [factory],
                IList<object> list => list.Cast<ViewModelFactory>(),
                _ => []
            }).ToArray();

        return _elementFactory.CreateElement(ElementDefinitionAdapter, adaptedProps, adaptedChildren);

        ViewModelFactory ElementDefinitionAdapter(IDictionary<string, object?> p, ViewModelFactory[] c) =>
            element(p, c);
    }

    public object[] useState(object initialState)
    {
        var (state, setState) = StateManager.UseState(initialState);
        return [state, setState];
    }
}
