namespace UiFramework.Elements;

public static class Elements
{
    public static ViewModelFactory Text(IDictionary<string, object?> props, ViewModelFactory[] children)
    {
        return () => new TextViewModel
        {
            Text = props.TryGetValue("Text", out var text)
                ? (string)text!
                : string.Empty
        };
    }

    public static ViewModelFactory Button(IDictionary<string, object?> props, ViewModelFactory[] children)
    {
        props.TryGetValue("OnClick", out var onClick);
        return () => new ButtonViewModel((Action?)onClick)
        {
            Text = props.TryGetValue("Text", out var text)
                ? (string)text!
                : string.Empty,
        };
    }

    public static ViewModelFactory Container(IDictionary<string, object?> props, ViewModelFactory[] children)
    {
        return () => new ContainerViewModel(children.Select(child => child()).ToArray());
    }
}
