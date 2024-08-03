namespace UiFramework.Tests;

public class UiFrameworkTestFixture
{
    public void Init(IUiFrameworkTestSuite suite)
    {
        suite.ViewModel = new();
        suite.Controller = UiFactory.CreateRoot(new TestAppViewModel(), nameof(suite.ViewModel.Content));
    }
}

public interface IUiFrameworkTestSuite
{
    TestAppViewModel ViewModel { get; set; }
    RootController Controller { get; set; }
    void Render(ElementFactory element) => Controller.Render(element);

    T? Root<T>() => ViewModel.Content.As<T>();
}
