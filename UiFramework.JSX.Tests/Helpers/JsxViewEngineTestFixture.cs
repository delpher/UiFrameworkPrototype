namespace UiFramework.JSX.Tests.Helpers;

public class JsxViewEngineTestFixture : IDisposable
{
    public readonly TestViewModel ViewModel;
    public readonly JsxViewEngine ViewEngine;

    public JsxViewEngineTestFixture()
    {
        ViewModel = new();
        ViewEngine = new(Framework.CreateRoot(ViewModel, nameof(ViewModel.Content)));
    }

    public void Dispose()
    {
        ViewEngine.Dispose();
        GC.SuppressFinalize(this);
    }
}
