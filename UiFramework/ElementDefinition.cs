namespace UiFramework;

public delegate ViewModelFactory ElementDefinition(IDictionary<string, object?> props, params ViewModelFactory[] children);
