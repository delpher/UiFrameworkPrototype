namespace UiFramework.Primitives;

public class Elements
{
    public static ViewModelFactory Text(IDictionary<string, object?> props, ViewModelFactory[] children) =>
        () => new TextViewModel
        {
            Text = props.TryGetValue("text", out var text)
                ? text?.ToString()
                : string.Empty
        };

    public static ViewModelFactory Button(IDictionary<string, object?> props, ViewModelFactory[] children)
    {
        props.TryGetValue("onClick", out var onClick);
        return () => new ButtonViewModel(() => ((dynamic)onClick!)())
        {
            Text = props.TryGetValue("text", out var text)
                ? text?.ToString()
                : string.Empty
        };
    }

    public static ViewModelFactory Container(IDictionary<string, object?>? props, ViewModelFactory[] children) =>
        () => new ContainerViewModel(CommitChildren(children));

    private static object?[] CommitChildren(ViewModelFactory[] children) =>
        children
            .Select(child => child.Invoke())
            .Where(viewModel => viewModel != null)
            .ToArray();

    public static ViewModelFactory Fragment(IDictionary<string, object?> props, ViewModelFactory[] children) =>
        () =>
        {
            if (children.Length == 1) return children[0]();
            return children.Select(c => c())
                .Where(vm => vm != null).ToArray();
        };
}
