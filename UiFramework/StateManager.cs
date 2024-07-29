namespace UiFramework;

public class StateManager(IRootController rootController)
{
    private static StateManager? _current;
    private readonly List<object> _states = [];
    private int _stateIndex = -1;

    public void ResetIndex() => _stateIndex = -1;

    private (object, Action<object>) UseStateInternal(object initialState)
    {
        _stateIndex++;
        var memoizedStateIndex = _stateIndex;
        if (_stateIndex == _states.Count) _states.Add(initialState);
        return (_states[_stateIndex], value =>
        {
            _states[memoizedStateIndex] = value;
            rootController.Render();
        });
    }

    public static (object, Action<object>) UseState(object initialState)
    {
        if (_current == null)
            throw new InvalidOperationException("UseState can only be called inside component");

        return _current.UseStateInternal(initialState);
    }

    public static void SetCurrent(StateManager stateManager)
    {
        _current = stateManager;
    }
}
