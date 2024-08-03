using System.ComponentModel;

namespace UiFramework.Primitives;

public static class DynamicExtensions
{
    public static IDictionary<string, object?> GetProperties(this object? source) =>
        source == null ? new() : ObjectToDictionary(source);

    private static Dictionary<string, object?> ObjectToDictionary(object source) =>
        TypeDescriptor.GetProperties(source)
            .Cast<PropertyDescriptor>()
            .ToDictionary(
                p => p.Name,
                p => p.GetValue(source));
}
