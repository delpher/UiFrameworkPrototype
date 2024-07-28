namespace UiFramework.JSX;

public class FrameworkJsxAdapter(RootController rootController)
{
    private readonly ElementFactory _elementFactory = new(rootController);

    // ReSharper disable once InconsistentNaming
    public ViewModelFactory createElement(object? element, object? props, object? children)
    {
        var adaptedChildren = (IEnumerable<object>?)children;
        return createElement((Func<IDictionary<string, object?>, ViewModelFactory[], ViewModelFactory>)element!, props, adaptedChildren?.ToArray());
    }

    // ReSharper disable once InconsistentNaming
    public ViewModelFactory createElement(Func<IDictionary<string, object?>, ViewModelFactory[], ViewModelFactory> element, object? props, object[]? children = null)
    {
        var adaptedProps = (IDictionary<string, object?>)(props ?? new Dictionary<string, object?>());
        var adaptedChildren = (children ?? []).Cast<ViewModelFactory>().ToArray();

        return _elementFactory.CreateElement(ElementDefinitionAdapter, adaptedProps , adaptedChildren);

        ViewModelFactory ElementDefinitionAdapter(IDictionary<string, object?> p, ViewModelFactory[] c) => element(p, c);
    }

    public object[] useState()
    {
        throw new NotImplementedException();
    }
}
