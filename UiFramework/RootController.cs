namespace UiFramework;

public class RootController(Action<object> output) : IRootController
{
    private Action _renderer = null!;
    public void Render(ViewModelFactory element)
    {
        (_renderer = () => output(element()))();
    }

    public void Render() => _renderer();
}
