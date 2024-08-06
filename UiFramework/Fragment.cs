namespace UiFramework;

public static class Fragment
{
    public static Primitive CreateType()
    {
        return (_, children) =>
            () =>
            {
                if (children.Length == 1) return children[0]();
                return children.Select(c => c())
                    .Where(vm => vm != null).ToArray();
            };
    }

}
