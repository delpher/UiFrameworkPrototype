namespace UiFramework;

public class RootController(Action<object?> output)
{
    private Action _renderer = null!;
    private StateManager _stateManager = new();

    public void Render(Element element)
    {
        _stateManager = new();
        _renderer = CreateRenderer(element);
        _stateManager.OnStateUpdated(_renderer);
        _renderer();
    }

    private Action CreateRenderer(Element? element) =>
        () =>
        {
            _stateManager.Reset();
            using (StateManager.LockTo(_stateManager))
                output(RenderElement(element));
        };

    private object? RenderElement(Element? element)
    {
        _stateManager.AdvanceState();
        if (element == null) return null;

        var props = element.Props ?? new Dictionary<string, object?>();
        var children = element.Children ?? [];
        return element.Type switch
        {
            Primitive primitive => primitive(props, RenderChildren(children))(),
            Component component => RenderElement(component(props, children)),
            _ => throw new NotSupportedException($"Element of type '{element.GetType().Name}' is not supported")
        };
    }

    private ViewFactory[] RenderChildren(Element?[] children) => children.Select(RenderChild).ToArray();

    private ViewFactory RenderChild(Element? child) => () => RenderElement(child);
}
