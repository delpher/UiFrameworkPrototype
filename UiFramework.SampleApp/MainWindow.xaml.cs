using static UiFramework.Framework;
using static UiFramework.WPF.Elements;

namespace UiFramework.SampleApp;

public partial class MainWindow
{
    public MainWindow()
    {
        InitializeComponent();

        CreateRoot(this, nameof(DataContext))
            .Render(CreateElement(App));
    }

    private Element App(IDictionary<string, object?>? props, Element?[] children)
    {
        var (file, setFile) = UseState("no file selected");

        return CreateElement(Container, null,
            CreateElement(Text, new { text = file }),
            CreateElement(FileInput, new { selectedFile = file, onChange = new Action<string>(path => setFile(path)) })
        );
    }
}
