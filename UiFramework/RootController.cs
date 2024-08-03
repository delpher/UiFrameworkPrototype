using System.Diagnostics;

namespace UiFramework;

public class StateManager
{
    private readonly List<object?> _states = [];
    private int _stateIndex = -1;

    private static StateManager? _current;
    private Action _renderer = () => { };

    public static void SetCurrent(StateManager stateManager)
    {
        _current = stateManager;
    }

    public static (object?, Action<object>) UseState(object initialState)
    {
        return _current!.UseStateImpl(initialState);
    }

    private (object?, Action<object?>) UseStateImpl(object initialState)
    {
        var memoizedIndex = _stateIndex;

        _states[memoizedIndex] ??= initialState;

        return (_states[memoizedIndex], state =>
        {
            _states[memoizedIndex] = state;
            _renderer();
        });
    }

    public void AdvanceState()
    {
        _stateIndex++;
        if (_states.Count == _stateIndex) _states.Add(null);
    }

    public void Reset()
    {
        _stateIndex = -1;
    }

    public void SetRenderer(Action renderer)
    {
        _renderer = renderer;
    }
}

public class RootController(Action<object> output)
{
    private Action _renderer = null!;
    private StateManager _stateManager = new();

    public void Render(ElementFactory elementFactory)
    {
        _stateManager = new();
        StateManager.SetCurrent(_stateManager);
        _renderer = () =>
        {
            _stateManager.Reset();
            output(CreateViewModel(elementFactory()));
        };
        _stateManager.SetRenderer(_renderer);
        _renderer();
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
