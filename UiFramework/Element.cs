namespace UiFramework;

public class Element
{
    public object? Type { get; set; }
    public IDictionary<string, object?>? Props { get; set; }
    public ElementFactory?[]? Children { get; set; }
}