namespace UiFramework;

public delegate ViewFactory Primitive(IDictionary<string, object?> props, params ViewFactory[] children);