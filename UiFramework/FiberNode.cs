namespace UiFramework;

public class FiberNode(Func<object> viewModelFactory)
{
    public object Execute()
    {
        return viewModelFactory();
    }
}

