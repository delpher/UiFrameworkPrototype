using UiFramework.Hooks;

namespace UiFramework;

public static class Framework
{
    public static RootController CreateRoot(object viewModel, string propertyName) =>
        new(content => viewModel.GetType().GetProperty(propertyName)!.SetValue(viewModel, content));

    public static Element CreateElement(
        Component component,
        IDictionary<string, object?>? props,
        params Element?[] children) =>
        new()
        {
            Type = component,
            Props = props,
            Children = children
        };

    public static Element CreateElement(
        Primitive primitive,
        IDictionary<string, object?>? props,
        params Element?[] children) =>
        new()
        {
            Type = primitive,
            Props = props,
            Children = children
        };

    public static Element CreateElement(
        Component component,
        dynamic? props = null,
        params Element?[] children) =>
        CreateElement(component, DynamicExtensions.GetProperties(props), children);

    public static Element CreateElement(
        Primitive primitive,
        dynamic? props = null,
        params Element?[] children) =>
        CreateElement(primitive, DynamicExtensions.GetProperties(props), children);

    public static (object?, Action<object?>) UseState(object? initialState) =>
        UseStateHook.UseState(initialState);

    public static (T?, Action<T?>) UseState<T>(T? initialState) =>
        UseStateHook.UseState(initialState);

    public static void UseEffect(Action effect, object[] dependencies) =>
        UseEffectHook.UseEffect(effect, dependencies);

    public static Context CreateContext() =>
        UseContextHook.CreateContext();

    public static object? UseContext(Context context) =>
        UseContextHook.UseContext(context);

    public static readonly Primitive Fragment = UiFramework.Fragment.CreateType();
}
