namespace UiFramework;

public static class DependenciesComparer
{
    public static bool AreSame(object?[]? first, object?[]? second)
    {
        if (first == null || second == null) return false;
        if (first.Length != second.Length) throw new InvalidOperationException();
        return first.Length != 0 &&
               first.Select((item, index) =>
                   Equals(item, second[index])).All(result => result);
    }
}
