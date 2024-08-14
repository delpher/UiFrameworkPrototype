using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Microsoft.ClearScript;
using Microsoft.ClearScript.V8;

namespace WpfWithReact;

public partial class MainWindow
{
    public MainWindow()
    {
        var jsEngine = new V8ScriptEngine();
        DataContext = new MainWindowViewModel();
        jsEngine.AddHostObject("__WpfHost__", new WpfViewHost(DataContext));
        jsEngine.Script._timeout = new Action<ScriptObject, int>((callback, delay) =>
        {
            var t = new Timer(_ => callback.Invoke(false));
            t.Change(delay, Timeout.Infinite);
        });
        jsEngine.Execute("setTimeout = (callback, delay) => _timeout(callback, delay)");
        jsEngine.Execute(EmbeddedResources.Read("dist.index.js"));
        InitializeComponent();
    }
}

public class MainWindowViewModel: IHaveChildren
{
    public ObservableCollection<object> Children { get; } = [];
    public void Append(object child) => Children.Add(child);
    public void ClearChildren() => Children.Clear();
}

public class WpfViewHost(object root)
{
    public object Root { get; } = root;

    public object CreateInstance(object type, dynamic props, object rootContainer)
    {
        if (type is "text")
            return new TextViewModel(props.children as string);

        if (type is "button")
            return new ButtonViewModel(props.children as string, (Action)props.onClick);

        throw new NotSupportedException($"'{type}' is not supported");
    }

    public object CreateTextInstance(object text, object rootContainer)
    {
        return text;
    }

    public void AppendChildToContainer(object container, object child)
    {
        if (container is IHaveChildren parent) parent.Append(child);
    }

    public void AppendChild(object parent, object child)
    {
        Debug.WriteLine("NOOP");
    }

    public void AppendInitialChild(object parent, object child)
    {
        Debug.WriteLine("NOOP");
    }

    public void PrepareForCommit(object args)
    {
        Debug.WriteLine("NOOP");
    }

    public void ResetAfterCommit(object args)
    {
        Debug.WriteLine("NOOP");
    }

    public void ClearContainer(object container)
    {
        if (container is IHaveChildren containerWithChildren)
            containerWithChildren.ClearChildren();
    }

    public bool ShouldSetTextContent(object type, dynamic props)
    {
        return type is "text" or "button";
    }

    public bool FinalizeInitialChildren(object instance, object type, dynamic props, object rootContainer,
        object hostContext)
    {
        Debug.WriteLine("NOOP");
        return false;
    }

    public void ResetTextContent(object instance)
    {
        Debug.WriteLine("NOOP");
    }

    public void CommitUpdate(object instance, object hostContext, object type, dynamic prevProps, dynamic nextProps, object internalHandle)
    {
        if (instance is TextViewModel textViewModel)
            textViewModel.Text = nextProps.children as string;
        Debug.WriteLine("NOOP");
    }

    public void CommitTextUpdate(object textInstance, string prevText, string nextText)
    {
        Debug.WriteLine("NOOP");
    }

    public object PrepareUpdate(object args)
    {
        Debug.WriteLine("NOOP");
        return new { id = "some context" };
    }
}

public interface IHaveChildren
{
    void Append(object child);
    void ClearChildren();
}

public class ButtonViewModel(string? text, Action? onClick)
{
    public string? Text => text;
    public ICommand Click => new ActionCommand(onClick);
}

public class ActionCommand(Action? action) : ICommand
{
    public bool CanExecute(object? parameter)
    {
        return action != null;
    }

    public void Execute(object? parameter)
    {
        action?.Invoke();
    }

    public event EventHandler? CanExecuteChanged;
}

public class TextViewModel(string? initialText): INotifyPropertyChanged
{
    public string? Text
    {
        get => initialText;
        set => SetField(ref initialText, value);
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null) =>
        PropertyChanged?.Invoke(this, new(propertyName));

    private void SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return;
        field = value;
        OnPropertyChanged(propertyName);
    }
}
