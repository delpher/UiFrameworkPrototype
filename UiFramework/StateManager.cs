namespace UiFramework;

public class StateManager
{
    private static readonly Mutex Lock = new();
    private static StateManager? _current;

    private readonly Dictionary<int, object?> _states = [];
    private int _stateIndex = -1;
    private Action _onStateUpdated = () => { };
    private bool _stateAbandoned = true;

    public static IDisposable LockTo(StateManager stateManager) {
        Lock.WaitOne();
        _current = stateManager;
        return new StateManagerRent();
    }

    private class StateManagerRent : IDisposable
    {
        public void Dispose() => Lock.ReleaseMutex();
    }

    public static (object?, Action<object>) UseState(object initialState) =>
        _current!.UseStateImpl(initialState);

    private (object?, Action<object?>) UseStateImpl(object initialState)
    {
        _stateAbandoned = false;
        var memoizedIndex = _stateIndex;

        _states.TryAdd(memoizedIndex, initialState);

        return (_states[memoizedIndex], state =>
        {
            _states[memoizedIndex] = state;
            _onStateUpdated();
        });
    }

    public void AdvanceState()
    {
        if (_stateAbandoned) _states.Remove(_stateIndex);
        _stateAbandoned = true;
        _stateIndex++;
    }

    public void Reset() => _stateIndex = -1;

    public void OnStateUpdated(Action handler) => _onStateUpdated = handler;
}
