﻿namespace UiFramework.Elements;

public class ContainerViewModel
{
    public ContainerViewModel(object?[] children)
    {
        Children = children;
    }

    public object?[] Children { get; }
}
