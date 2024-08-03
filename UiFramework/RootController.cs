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

    private Action CreateRenderer(ElementFactory elementFactory)
    {
        return () =>
        {
            _stateManager.Reset();
            using (StateManager.LockTo(_stateManager))
                output(CreateViewModel(elementFactory()));
        };
    }

    private object? CreateViewModel(Element? element)
    {
        _stateManager.AdvanceState();
        if (element == null) return null;

        var props = element.Props ?? new Dictionary<string, object?>();
        var children = element.Children ?? [];
        switch (element.Type)
        {
            case Primitive primitive:
            {
                var viewModelFactories = children
                    .Select(child =>
                        new ViewModelFactory(() => CreateViewModel(child?.Invoke())))
                    .ToArray();
                return primitive(props, viewModelFactories)();
            }
            case Component component:
                return CreateViewModel(component(props, children)());
            default:
                throw new NotSupportedException($"Element of type '{element.GetType().Name}' is not supported");
        }
    }

    public void Render() => _renderer();
}
