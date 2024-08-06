namespace UiFramework;

public class Context
{
    private object? _value;
    public Component Provider { get; }

    public Context()
    {
        Provider = (props, children) =>
        {
            _value = props["value"];
            return Framework.CreateElement(Framework.Fragment, null, children);
        };
    }

    public object? GetValue() => _value;
}
