namespace UiFramework.Hooks;

public static class UseContextHook
{
    public static object? UseContext(Context context) => context.GetValue();

    public static Context CreateContext() => new();
}
