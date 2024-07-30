using UiFramework.Elements;

namespace UiFramework;

public class ElementFactory(RootController rootController)
{
    public FiberNode CreateElement(
        ElementDefinition element,
        IDictionary<string, object?>? props,
        params FiberNode[] children)
    {
        return new(() =>
        {
            rootController.SetCurrent();
            return element(props ?? new Dictionary<string, object?>(), children).Execute();
        });
    }

    public FiberNode CreateElement(
        ElementDefinition element,
        dynamic? props = null,
        params FiberNode[] children) =>
        CreateElement(element, DynamicExtensions.GetProperties(props), children);
}
