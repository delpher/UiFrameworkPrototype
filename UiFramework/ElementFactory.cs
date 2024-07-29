using UiFramework.Elements;

namespace UiFramework;

public class ElementFactory(RootController rootController)
{
    public ViewModelFactory CreateElement(
        ElementDefinition element,
        IDictionary<string, object?>? props,
        params ViewModelFactory[] children)
    {
        return () =>
        {
            rootController.MakeCurrent();
            return element(props ?? new Dictionary<string, object?>(), children)();
        };
    }

    public ViewModelFactory CreateElement(ComponentDefinition element, dynamic? props, ViewModelFactory[] children)
    {
        var stateManager = new StateManager(rootController);
        return () =>
        {
            rootController.MakeCurrent();
            return element(stateManager, props ?? new Dictionary<string, object?>(), children)();
        };
    }

    public ViewModelFactory CreateElement(
        ElementDefinition element,
        dynamic? props = null,
        params ViewModelFactory[] children) =>
        CreateElement(element, DynamicExtensions.GetProperties(props), children);
}
