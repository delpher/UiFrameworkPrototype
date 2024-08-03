namespace UiFramework;

public class Element
{
    public object? Type { get; init; }
    public IDictionary<string, object?>? Props { get; init; }
    public ElementFactory?[]? Children { get; init; }
}
