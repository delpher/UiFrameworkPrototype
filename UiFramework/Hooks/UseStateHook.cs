namespace UiFramework.Hooks;

public static class UseStateHook
{
    public static (object?, Action<object?>) UseState(object? initialState) => StateManager.UseState(initialState);

    public static (T?, Action<T?>) UseState<T>(T? initialState)
    {
        var (state, update) = UseState(initialState as object);
        return ((T?)state, s => update(s));
    }
}
