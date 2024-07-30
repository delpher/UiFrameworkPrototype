using UiFramework.Elements;

namespace UiFramework;

public class ElementFactory
{
    public Element CreateElement(
        Component component,
        IDictionary<string, object?>? props,
        params Element[] children)
    {
        return () => component(props ?? new Dictionary<string, object?>(), children)();
    }

    public Element CreateElement(
        Component component,
        dynamic? props = null,
        params Element[] children) =>
        CreateElement(component, DynamicExtensions.GetProperties(props), children);
}
