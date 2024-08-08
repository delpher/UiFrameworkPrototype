using UiFramework.WPF;

namespace UiFramework.JSX.SampleApp;

public partial class MainWindow
{
    public MainWindow()
    {
        InitializeComponent();
        var viewEngine = new JsxViewEngine(Framework.CreateRoot(this, nameof(DataContext)));
        viewEngine.ExposeComponents(typeof(Elements));
        viewEngine.Render(App.ReadResource("index.jsx"));
    }
}
