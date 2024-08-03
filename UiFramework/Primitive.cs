namespace UiFramework;

public delegate ViewModelFactory Primitive(IDictionary<string, object?> props, params ViewModelFactory[] children);