namespace UiFramework.WPF;

public static class Elements
{
    public static ViewFactory Text(IDictionary<string, object?> props, ViewFactory[] children) =>
        () => new TextViewModel
        {
            Text = props.TryGetValue("text", out var text)
                ? text?.ToString()
                : string.Empty
        };

    public static ViewFactory FileInput(IDictionary<string, object?> props, ViewFactory[] children) =>
        () =>
        {
            props.TryGetValue("onChange", out var onChange);
            props.TryGetValue("selectedFile", out var selectedFile);
            return new FileInputViewModel(value => ((dynamic)onChange!)(value))
            {
                SelectedFile = selectedFile as string ?? ""
            };
        };

    public static ViewFactory Button(IDictionary<string, object?> props, ViewFactory[] children)
    {
        props.TryGetValue("onClick", out var onClick);
        return () => new ButtonViewModel(() => ((dynamic)onClick!)())
        {
            Text = props.TryGetValue("text", out var text)
                ? text?.ToString()
                : string.Empty
        };
    }

    public static ViewFactory Container(IDictionary<string, object?>? props, ViewFactory[] children) =>
        () => new ContainerViewModel(CommitChildren(children));

    private static object?[] CommitChildren(ViewFactory[] children) =>
        children
            .Select(child => child.Invoke())
            .Where(viewModel => viewModel != null)
            .ToArray();
}
