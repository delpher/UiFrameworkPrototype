using System.Reflection;

namespace UiFramework.Elements;

public class Elements
{
    public bool Exposes(object element)
    {
        var methods = typeof(Elements).GetMethods(BindingFlags.Public | BindingFlags.Static);
        return methods.Any(info =>
            Equals(info.CreateDelegate<Func<IDictionary<string, object?>, FiberNode[], FiberNode>>(), element));
    }

    public static FiberNode Text(IDictionary<string, object?> props, FiberNode[] children)
    {
        return new(() => new TextViewModel
        {
            Text = props.TryGetValue("Text", out var text)
                ? text?.ToString()
                : string.Empty
        });
    }

    public static FiberNode Button(IDictionary<string, object?> props, FiberNode[] children)
    {
        props.TryGetValue("OnClick", out var onClick);
        return new(() => new ButtonViewModel(() => ((dynamic)onClick!)())
        {
            Text = props.TryGetValue("Text", out var text)
                ? text?.ToString()
                : string.Empty,
        });
    }

    public static FiberNode Container(IDictionary<string, object?> props, FiberNode[] children)
    {
        return new(() =>
            new ContainerViewModel(
                children.Select(child => child.Execute()).ToArray()
            ));
    }
}
