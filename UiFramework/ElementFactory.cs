using UiFramework.Elements;

namespace UiFramework;

public class ElementFactory
{
    public Element CreateElement(
        ElementDefinition element,
        IDictionary<string, object?>? props,
        params Element[] children)
    {
        return () => element(props ?? new Dictionary<string, object?>(), children)();
    }

    public Element CreateElement(
        ElementDefinition element,
        dynamic? props = null,
        params Element[] children) =>
        CreateElement(element, DynamicExtensions.GetProperties(props), children);
}
