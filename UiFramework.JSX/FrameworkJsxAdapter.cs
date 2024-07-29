namespace UiFramework.JSX;

// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global
// ReSharper disable MemberCanBePrivate.Global
public class FrameworkJsxAdapter(RootController rootController)
{
    private readonly ElementFactory _elementFactory = new(rootController);

    public void beginComponent()
    {
        rootController.MakeCurrent();
    }

    public ViewModelFactory createElement(object? element, object? props, object? children)
    {
        var adaptedChildren = (IEnumerable<object>?)children;
        return createElement((Func<IDictionary<string, object?>, ViewModelFactory[], ViewModelFactory>)element!, props, adaptedChildren?.ToArray());
    }

    public ViewModelFactory createElement(Func<IDictionary<string, object?>, ViewModelFactory[], ViewModelFactory> element, object? props, object[]? children = null)
    {
        var adaptedProps = (IDictionary<string, object?>)(props ?? new Dictionary<string, object?>());
        var adaptedChildren = (children ?? []).Cast<ViewModelFactory>().ToArray();

        return _elementFactory.CreateElement(ElementDefinitionAdapter, adaptedProps , adaptedChildren);

        ViewModelFactory ElementDefinitionAdapter(IDictionary<string, object?> p, ViewModelFactory[] c) => element(p, c);
    }

    public static object[] useState(object initialState)
    {
        var (state, setState) = StateManager.UseState(initialState);
        return [state, setState];
    }
}
