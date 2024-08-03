using UiFramework.Elements;

namespace UiFramework;

public static class Framework
{
    public static ElementFactory CreateElement(
        Component component,
        IDictionary<string, object?>? props,
        params ElementFactory[] children)
    {
        return () => new()
        {
            Type = component,
            Props = props,
            Children = children
        };
    }

    public static ElementFactory CreateElement(
        Primitive primitive,
        IDictionary<string, object?>? props,
        params ElementFactory[] children)
    {
        return () => new()
        {
            Type = primitive,
            Props = props,
            Children = children
        };
    }

    public static ElementFactory CreateElement(
        Component component,
        dynamic? props = null,
        params ElementFactory[] children) =>
        CreateElement(component, DynamicExtensions.GetProperties(props), children);

    public static ElementFactory CreateElement(
        Primitive primitive,
        dynamic? props = null,
        params ElementFactory[] children) =>
        CreateElement(primitive, DynamicExtensions.GetProperties(props), children);
}
