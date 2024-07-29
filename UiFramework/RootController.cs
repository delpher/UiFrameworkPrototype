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

    public void Render(ViewModelFactory element) =>
        (_renderer = () =>
        {
            _stateManager.ResetIndex();
            _output(element());
        })();

    public void Render() => _renderer();

    public void MakeCurrent() => StateManager.SetCurrent(_stateManager);
}
