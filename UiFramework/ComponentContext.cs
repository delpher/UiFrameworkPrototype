namespace UiFramework;

public class ComponentContext(IRootController root) : IComponentContext
{
    private object? _storage;

    public (object state, Action<object> setState) UseState(object initialState)
    {
        _storage ??= initialState;
        return (_storage, value =>
        {
            _storage = value;
            root.Render();
        });
    }
}
