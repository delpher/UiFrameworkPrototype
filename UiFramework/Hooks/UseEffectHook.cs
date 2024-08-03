namespace UiFramework.Hooks;

public static class UseEffectHook
{
    public static void UseEffect(Action effect, object[] dependencies)
    {
        var (state, update) = Framework.UseState<object?[]>(null);

        if (DependenciesComparer.AreSame(state, dependencies)) return;

        effect();
        update(dependencies);
    }
}
