namespace UiFramework;

public delegate ElementFactory Component(IDictionary<string, object?> props, params ElementFactory?[] children);
public delegate Element ElementFactory();
public delegate ViewModelFactory Primitive(IDictionary<string, object?> props, params ViewModelFactory[] children);

public class Element
{
    public object? Type { get; set; }
    public IDictionary<string, object?>? Props { get; set; }
    public ElementFactory?[]? Children { get; set; }
}
