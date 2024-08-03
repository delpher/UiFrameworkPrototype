namespace UiFramework;

public class RootController(Action<object?> output)
{
    private Action _renderer = null!;
    private StateManager _stateManager = new();

    public void Render(ElementFactory element)
    {
        _stateManager = new();
        _renderer = CreateRenderer(element);
        _stateManager.OnStateUpdated(_renderer);
        _renderer();
    }

    private Action CreateRenderer(ElementFactory elementFactory) =>
        () =>
        {
            _stateManager.Reset();
            using (StateManager.LockTo(_stateManager))
                output(Render(elementFactory()));
        };

    private object? Render(Element? element)
    {
        _stateManager.AdvanceState();
        if (element == null) return null;

        var props = element.Props ?? new Dictionary<string, object?>();
        var children = element.Children ?? [];
        return element.Type switch
        {
            Primitive primitive => primitive(props, RenderChildren(children))(),
            Component component => Render(component(props, children)()),
            _ => throw new NotSupportedException($"Element of type '{element.GetType().Name}' is not supported")
        };
    }

    private ViewModelFactory[] RenderChildren(ElementFactory?[] children) =>
        children.Select(RenderElement).ToArray();

    private ViewModelFactory RenderElement(ElementFactory? child) =>
        () => Render(child?.Invoke());
}
