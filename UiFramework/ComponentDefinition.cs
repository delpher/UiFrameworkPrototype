namespace UiFramework;

public delegate ViewModelFactory ComponentDefinition(IComponentContext context, dynamic props,
    params ViewModelFactory[] children);
