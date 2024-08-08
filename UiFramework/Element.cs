namespace UiFramework;

public class Element
{
    public object? Type { get; init; }
    public IDictionary<string, object?>? Props { get; init; }
    public Element?[]? Children { get; init; }
}
