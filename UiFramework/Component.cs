namespace UiFramework;

public delegate ElementFactory Component(IDictionary<string, object?> props, params ElementFactory?[] children);
