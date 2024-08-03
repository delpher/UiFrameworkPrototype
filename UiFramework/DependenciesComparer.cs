namespace UiFramework;

public static class DependenciesComparer
{
    public static bool AreSame(object?[]? first, object?[]? second)
    {
        if (first == null || second == null) return false;
        if (first.Length != second.Length) throw new InvalidOperationException();
        if (first.Length == 0) return false;
        return first.Select((item, index) => Equals(item, second[index])).All(result => result);
    }
}
