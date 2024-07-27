namespace UiFramework;

public interface IComponentContext
{
    (object state, Action<object> setState) UseState(object initialState);
}
