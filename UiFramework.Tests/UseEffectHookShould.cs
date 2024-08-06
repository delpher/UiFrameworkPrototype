using UiFramework.Primitives;
using UiFramework.Tests.Helpers;
using static UiFramework.Framework;
using static UiFramework.Primitives.Elements;

namespace UiFramework.Tests;

public class UseEffectHookShould
{
    private readonly TestAppViewModel _testAppViewModel;
    private readonly RootController _root;

    public UseEffectHookShould()
    {
        _testAppViewModel = new();
        _root = CreateRoot(_testAppViewModel, nameof(_testAppViewModel.Content));
    }

    private void Render(ElementFactory element) => _root.Render(element);
    private T Root<T>() => _testAppViewModel.Content.As<T>()!;

    [Fact]
    public void Update_On_Dependency_Change()
    {
        List<int> log = [];

        Render(CreateElement(App));

        log.Should().HaveCount(1).And.Contain(0);

        Root<ContainerViewModel>().Children[0].As<ButtonViewModel>().Click.Execute(null);
        log.Should().HaveCount(1).And.Contain(0);

        Root<ContainerViewModel>().Children[0].As<ButtonViewModel>().Click.Execute(null);
        log.Should().HaveCount(1).And.Contain(0);

        Root<ContainerViewModel>().Children[1].As<ButtonViewModel>().Click.Execute(null);
        log.Should().HaveCount(2).And.Contain([0, 1]);

        Root<ContainerViewModel>().Children[1].As<ButtonViewModel>().Click.Execute(null);
        log.Should().HaveCount(2).And.Contain([0, 1]);

        Root<ContainerViewModel>().Children[0].As<ButtonViewModel>().Click.Execute(null);
        log.Should().HaveCount(3).And.Contain([0, 1, 0]);
        return;

        ElementFactory App(dynamic? props, ElementFactory?[] children)
        {
            var (version, setVersion) = UseState(0);

            UseEffect(() => log.Add(version), [version]);

            return CreateElement(Container, null,
                CreateElement(Button, new { onClick = new Action(() => setVersion(0)) }),
                CreateElement(Button, new { onClick = new Action(() => setVersion(1)) })
            );
        }
    }
}
