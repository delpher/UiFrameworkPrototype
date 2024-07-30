using UiFramework.Elements;

namespace UiFramework;

public class ElementFactory
{
    public ViewModelFactory CreateElement(
        ElementDefinition element,
        IDictionary<string, object?>? props,
        params ViewModelFactory[] children)
    {
        return () => element(props ?? new Dictionary<string, object?>(), children)();
    }

    public ViewModelFactory CreateElement(
        ElementDefinition element,
        dynamic? props = null,
        params ViewModelFactory[] children) =>
        CreateElement(element, DynamicExtensions.GetProperties(props), children);
}
