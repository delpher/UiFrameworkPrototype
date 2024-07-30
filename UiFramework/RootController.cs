namespace UiFramework;

public class RootController(Action<object> output) : IRootController
{
    private Action _renderer = null!;
    private FiberNode? _rootFiber;

    public void Render(ViewModelFactory element)
    {
        _rootFiber = new(element, this);
        (_renderer = () => output(_rootFiber.Execute()))();
    }

    public void Render() => _renderer();
}
