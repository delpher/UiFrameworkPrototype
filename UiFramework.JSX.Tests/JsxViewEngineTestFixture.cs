namespace UiFramework.JSX.Tests;

public class JsxViewEngineTestFixture : IDisposable
{
    public readonly TestViewModel ViewModel;
    public readonly JsxViewEngine ViewEngine;

    public JsxViewEngineTestFixture()
    {
        ViewModel = new();
        ViewEngine = new(UiFactory.CreateRoot(ViewModel, nameof(ViewModel.Content)));
    }

    public void Dispose()
    {
        ViewEngine.Dispose();
        GC.SuppressFinalize(this);
    }
}