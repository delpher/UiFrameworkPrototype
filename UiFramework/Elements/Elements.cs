using System.Reflection;

namespace UiFramework.Elements;

public class Elements
{
    public bool Exposes(object element)
    {
        var methods = typeof(Elements).GetMethods(BindingFlags.Public | BindingFlags.Static);
        return methods.Any(info => Equals(info.CreateDelegate<Func<IDictionary<string, object?>, ViewModelFactory[], ViewModelFactory>>(), element));
    }

    public static ViewModelFactory Text(IDictionary<string, object?> props, ViewModelFactory[] children)
    {
        return () => new TextViewModel
        {
            Text = props.TryGetValue("Text", out var text)
                ? text?.ToString()
                : string.Empty
        };
    }

    public static ViewModelFactory Button(IDictionary<string, object?> props, ViewModelFactory[] children)
    {
        props.TryGetValue("OnClick", out var onClick);
        return () => new ButtonViewModel(() => ((dynamic)onClick!)())
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
