namespace UiFramework.SampleApp;

public partial class MainWindow
{
    public MainWindow()
    {
        InitializeComponent();
        var viewModel = new MainWindowViewModel();
        DataContext = viewModel;
        var viewEngine = new JSX.JsxViewEngine(UiFactory.CreateRoot(viewModel, nameof(viewModel.Content)));
        viewEngine.Render(App.ReadResource("index.jsx"));
    }
}
