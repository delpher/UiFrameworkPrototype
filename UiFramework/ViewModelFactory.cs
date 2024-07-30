namespace UiFramework;

public delegate object ViewModelFactory();

public class FiberNode(ViewModelFactory viewModelFactory, RootController rootController)
{
    private readonly StateManager _stateManager = new(rootController);
    public object Execute()
    {
        _stateManager.ResetIndex();
        StateManager.SetCurrent(_stateManager);
        return viewModelFactory();
    }
}
