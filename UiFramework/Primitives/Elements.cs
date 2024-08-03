namespace UiFramework.Primitives;

public class Elements
{
    public static ViewModelFactory Text(IDictionary<string, object?> props, ViewModelFactory[] children)
    {
        return () => new TextViewModel
        {
            Text = props.TryGetValue("text", out var text)
                ? text?.ToString()
                : string.Empty
        };
    }

    public static ViewModelFactory Button(IDictionary<string, object?> props, ViewModelFactory[] children)
    {
        props.TryGetValue("onClick", out var onClick);
        return () => new ButtonViewModel(() => ((dynamic)onClick!)())
        {
            Text = props.TryGetValue("text", out var text)
                ? text?.ToString()
                : string.Empty,
        };
    }

    public static ViewModelFactory Container(IDictionary<string, object?>? props, ViewModelFactory[] children)
    {
        return () =>
            new ContainerViewModel(
                children.Select(child => child.Invoke())
                    .Where(viewModel => viewModel != null).ToArray()
            );
    }
}
