using UiFramework.Hooks;
using UiFramework.Primitives;

namespace UiFramework;

public static class Framework
{
    public static RootController CreateRoot(object viewModel, string propertyName) =>
        new(content => viewModel.GetType().GetProperty(propertyName)!.SetValue(viewModel, content));

    public static ElementFactory CreateElement(
        Component component,
        IDictionary<string, object?>? props,
        params ElementFactory[] children) =>
        () => new()
        {
            Type = component,
            Props = props,
            Children = children
        };

    public static ElementFactory CreateElement(
        Primitive primitive,
        IDictionary<string, object?>? props,
        params ElementFactory?[] children) =>
        () => new()
        {
            Type = primitive,
            Props = props,
            Children = children
        };

    public static ElementFactory CreateElement(
        Component component,
        dynamic? props = null,
        params ElementFactory?[] children) =>
        CreateElement(component, DynamicExtensions.GetProperties(props), children);

    public static ElementFactory CreateElement(
        Primitive primitive,
        dynamic? props = null,
        params ElementFactory?[] children) =>
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

    public static readonly Primitive Fragment = Elements.Fragment;
}
