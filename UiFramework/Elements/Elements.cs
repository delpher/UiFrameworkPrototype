namespace UiFramework.Elements;

public class Elements
{
    public static Element Text(IDictionary<string, object?> props, Element[] children)
    {
        return () => new TextViewModel
        {
            Text = props.TryGetValue("Text", out var text)
                ? text?.ToString()
                : string.Empty
        };
    }

    public static Element Button(IDictionary<string, object?> props, Element[] children)
    {
        props.TryGetValue("OnClick", out var onClick);
        return () => new ButtonViewModel(() => ((dynamic)onClick!)())
        {
            Text = props.TryGetValue("Text", out var text)
                ? text?.ToString()
                : string.Empty,
        };
    }

    public static Element Container(IDictionary<string, object?> props, Element[] children)
    {
        return () =>
            new ContainerViewModel(
                children.Select(child => child()).ToArray()
            );
    }
}
