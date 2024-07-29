namespace UiFramework;

public delegate ViewModelFactory ElementDefinition(IDictionary<string, object?> props, params ViewModelFactory[] children);

public delegate ViewModelFactory ComponentDefinition(StateManager stateManager, IDictionary<string, object?> props,
    params ViewModelFactory[] children);
