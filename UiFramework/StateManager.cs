namespace UiFramework;

internal class StateManager
{
    private static readonly Mutex Lock = new();
    private static StateManager? _current;

    private readonly Dictionary<int, Dictionary<int, object?>> _contexts = [];
    private int _contextIndex = -1;
    private int _stateIndex = -1;
    private Action _onStateUpdated = () => { };
    private bool _stateAbandoned = true;

    public static IDisposable LockTo(StateManager stateManager)
    {
        Lock.WaitOne();
        _current = stateManager;
        return new StateManagerRent();
    }

    private class StateManagerRent : IDisposable
    {
        public void Dispose() => Lock.ReleaseMutex();
    }

    public static (object?, Action<object?>) UseState(object? initialState) =>
        _current!.UseStateImpl(initialState);

    private (object?, Action<object?>) UseStateImpl(object? initialState)
    {
        _stateAbandoned = false;
        _stateIndex++;

        if (!_contexts.ContainsKey(_contextIndex)) _contexts.Add(_contextIndex, new());

        var context = _contexts[_contextIndex];
        context.TryAdd(_stateIndex, initialState);

        var contextIndexMemo = _contextIndex;
        var stateIndexMemo = _stateIndex;
        return (_contexts[contextIndexMemo][stateIndexMemo], nextState =>
        {
            if (Equals(_contexts[contextIndexMemo][stateIndexMemo], nextState)) return;

            _contexts[contextIndexMemo][stateIndexMemo] = nextState;
            _onStateUpdated();
        });
    }

    public void AdvanceState()
    {
        if (_stateAbandoned) _contexts.Remove(_contextIndex);
        _stateAbandoned = true;
        _contextIndex++;
        _stateIndex = -1;
    }

    public void Reset()
    {
        _contextIndex = -1;
        _stateIndex = -1;
    }

    public void OnStateUpdated(Action handler) => _onStateUpdated = handler;
}
