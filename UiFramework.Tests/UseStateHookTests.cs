using FluentAssertions;
using UiFramework.Primitives;
using static UiFramework.Framework;
using static UiFramework.Primitives.Elements;

namespace UiFramework.Tests;

public class UseStateHookTests
{
    private readonly TestAppViewModel _testAppViewModel;
    private readonly RootController _root;

    public UseStateHookTests()
    {
        _testAppViewModel = new();
        _root = UiFactory.CreateRoot(_testAppViewModel, nameof(_testAppViewModel.Content));
    }

    private void Render(ElementFactory element) => _root.Render(element);

    private T Root<T>() => _testAppViewModel.Content.As<T>()!;

    [Fact]
    public void Single_State_Test()
    {
        Render(CreateElement(StateComponent));

        Root<ContainerViewModel>().Children[0].As<TextViewModel>().Text.Should().Be("initial text");
        Root<ContainerViewModel>().Children[1].As<ButtonViewModel>().Click.Execute(null);
        Root<ContainerViewModel>().Children[0].As<TextViewModel>().Text.Should().Be("button clicked");
        return;

        ElementFactory StateComponent(IDictionary<string, object?> props, params ElementFactory?[] elementFactories)
        {
            var (text, setText) = UseState("initial text");

            return CreateElement(Container, null,
                CreateElement(Text, new { text }),
                CreateElement(Button, new { onClick = new Action(() => setText("button clicked")) })
            );
        }
    }

    [Fact]
    public void Elements_List()
    {
        Render(CreateElement(StateComponent));

        Root<ContainerViewModel>().Children[^1].As<ButtonViewModel>().Click.Execute(null);
        Root<ContainerViewModel>().Children[0].As<TextViewModel>().Text.Should().Be("0");

        Root<ContainerViewModel>().Children[^1].As<ButtonViewModel>().Click.Execute(null);
        Root<ContainerViewModel>().Children[0].As<TextViewModel>().Text.Should().Be("0");
        Root<ContainerViewModel>().Children[1].As<TextViewModel>().Text.Should().Be("1");
        return;

        ElementFactory StateComponent(IDictionary<string, object?> props, params ElementFactory?[] elementFactories)
        {
            var (items, setItems) = UseState(Array.Empty<string>());

            return CreateElement(Container, null,
                items!.Select(item => CreateElement(Text, new { text = item }))
                    .Concat([
                        CreateElement(Button,
                            new
                            {
                                onClick = new Action(() =>
                                    setItems(items!.Concat([(items!.Length).ToString()]).ToArray()))
                            })
                    ])
                    .ToArray());
        }
    }

    [Fact]
    public void State_Management_Test()
    {
        Render(CreateElement(RootComponent));

        Root<ContainerViewModel>().Children[^1].As<ButtonViewModel>().Click.Execute(null);
        Root<ContainerViewModel>().Children[0].As<TextViewModel>().Text.Should().Be("B");
        return;

        ElementFactory ItemComponent(IDictionary<string, object?> props, params ElementFactory?[] elementFactories)
        {
            var (state, _) = UseState((string)props["name"]!);
            return CreateElement(Text, new { text = state });
        }

        ElementFactory RootComponent(dynamic? props, params ElementFactory?[] elementFactories)
        {
            var (showA, setShowA) = UseState(true);

            return CreateElement(Container, null,
                showA ? CreateElement(ItemComponent, new { name = "A" }) : null,
                CreateElement(ItemComponent, new { name = "B" }),
                CreateElement(Button, new { onClick = new Action(() => setShowA(false)) })
            );
        }
    }

    [Fact]
    public void State_Cleanup_Test()
    {
        Render(CreateElement(RootComponent));

        var initialText = Root<ContainerViewModel>().Children[0].As<TextViewModel>().Text;
        Root<ContainerViewModel>().Children[^1].As<ButtonViewModel>().Click.Execute(null);
        Root<ContainerViewModel>().Children[^1].As<ButtonViewModel>().Click.Execute(null);
        Root<ContainerViewModel>().Children[0].As<TextViewModel>().Text.Should().NotBe(initialText);
        return;

        ElementFactory ItemComponent(IDictionary<string, object?> props, params ElementFactory?[] elementFactories)
        {
            var (state, _) = UseState(Guid.NewGuid().ToString());
            return CreateElement(Text, new { text = state });
        }

        ElementFactory RootComponent(dynamic? props, params ElementFactory?[] elementFactories)
        {
            var (show, setShow) = UseState(true);

            return CreateElement(Container, null,
                show ? CreateElement(ItemComponent) : null,
                CreateElement(Button, new { onClick = new Action(() => setShow(!show)) })
            );
        }
    }
}
