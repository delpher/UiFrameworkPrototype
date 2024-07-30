namespace UiFramework;

public class RootController : IRootController
{
    private Action _renderer = null!;
    private readonly StateManager _stateManager;
    private readonly Action<object> _output;
    public RootController(Action<object> output)
    {
        _output = output;
        _stateManager = new(this);
    }

    public void Render(FiberNode element) =>
        (_renderer = () =>
        {
            _stateManager.ResetIndex();
            _output(element.Execute());
        })();

    public void Render() => _renderer();

    public void SetCurrent()
    {
        StateManager.SetCurrent(_stateManager);
    }

    public object CreateNode()
    {
        return new { };
    }
}
