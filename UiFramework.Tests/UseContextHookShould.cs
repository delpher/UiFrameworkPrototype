using UiFramework.Primitives;
using UiFramework.Tests.Helpers;
using static UiFramework.Framework;
using static UiFramework.Primitives.Elements;

namespace UiFramework.Tests;

public class UseContextHookShould
{
    private readonly TestAppViewModel _testAppViewModel;
    private readonly RootController _root;

    public UseContextHookShould()
    {
        _testAppViewModel = new();
        _root = UiFactory.CreateRoot(_testAppViewModel, nameof(_testAppViewModel.Content));
    }

    private void Render(ElementFactory element) => _root.Render(element);
    private T Root<T>() => _testAppViewModel.Content.As<T>()!;

    [Fact]
    public void Update_On_Provider_Value_Change()
    {
        var TestContext = CreateContext();

        Render(CreateElement(App));

        Root<ContainerViewModel>()
            .Children[0].As<TextViewModel>().Text.Should().Be("initial state");

        Root<ContainerViewModel>()
            .Children[1].As<ButtonViewModel>().Click.Execute(null);

        Root<ContainerViewModel>()
            .Children[0].As<TextViewModel>().Text.Should().Be("updated state");

        return;

        ElementFactory TestContextConsumer(dynamic? props, ElementFactory?[] children)
        {
            var value = UseContext(TestContext);
            return CreateElement(Text, new { text = value });
        }

        ElementFactory App(dynamic? props, ElementFactory?[] children)
        {
            var (state, setState) = UseState("initial state");

            return CreateElement(TestContext.Provider, new { value = state },
                CreateElement(Container, null,
                    CreateElement(TestContextConsumer),
                        CreateElement(Button, new { onClick = new Action(() => setState("updated state"))})
                    )
            );
        }
    }
}
