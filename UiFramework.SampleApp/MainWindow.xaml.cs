using static UiFramework.Elements.Elements;

namespace UiFramework.SampleApp;

public partial class MainWindow
{
    public MainWindow()
    {
        InitializeComponent();
        var viewModel = new MainWindowViewModel();
        DataContext = viewModel;
        var r = ViewFactory.CreateRoot(viewModel, nameof(viewModel.Content));
        var f = new ElementFactory(r);

        r.Render(f.CreateElement(MainComponent));

        return;

        ViewModelFactory MainComponent(IComponentContext context, dynamic props, params ViewModelFactory[] children)
        {
            var (text, setText) = context.UseState("Sample text");

            return f.CreateElement(Container, null,
                f.CreateElement(Text, new { Text = text }),
                f.CreateElement(Button,
                    new { Text = "Click me!", OnClick = new Action(() => setText("Button clicked!")) })
            );
        }
    }
}
