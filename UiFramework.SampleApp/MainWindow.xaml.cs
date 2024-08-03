using UiFramework.JSX;

namespace UiFramework.SampleApp;

public partial class MainWindow
{
    public MainWindow()
    {
        InitializeComponent();
        var viewModel = new MainWindowViewModel();
        DataContext = viewModel;
        var viewEngine = new JSX.Primitives(UiFactory.CreateRoot(viewModel, nameof(viewModel.Content)));
        viewEngine.Render(App.ReadResource("index.jsx"));
    }
}
