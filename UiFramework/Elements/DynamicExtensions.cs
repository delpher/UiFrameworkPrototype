﻿using System.ComponentModel;

namespace UiFramework.Elements;

public static class DynamicExtensions
{
    public static Dictionary<string, object?> GetProperties(this object? source)
    {
        if (source == null) return new();
        return TypeDescriptor.GetProperties(source).Cast<PropertyDescriptor>()
            .ToDictionary(p => p.Name, p => p.GetValue(source));
    }
}
