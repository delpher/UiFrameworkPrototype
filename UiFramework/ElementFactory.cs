using UiFramework.Elements;

namespace UiFramework;

public class ElementFactory(IRootController root)
{
    public ViewModelFactory CreateElement(
        ElementDefinition element,
        dynamic? props = null,
        params ViewModelFactory[] children)
    {
        return () => element(DynamicExtensions.GetProperties(props), children)();
    }

    public ViewModelFactory CreateElement(
        ElementDefinition element,
        IDictionary<string, object?>? props,
        params ViewModelFactory[] children)
    {
        return () => element(props ?? new Dictionary<string, object?>(), children)();
    }

    public ViewModelFactory CreateElement(
        ComponentDefinition component,
        dynamic? props = null,
        params ViewModelFactory[] children)
    {
        var context = new ComponentContext(root);
        return () => component(context, DynamicExtensions.GetProperties(props), children)();
    }
}
