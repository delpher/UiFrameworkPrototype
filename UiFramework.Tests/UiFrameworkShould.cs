using FluentAssertions;
using UiFramework.Elements;
using static UiFramework.Elements.Components;
using static UiFramework.StateManager;

namespace UiFramework.Tests;

public class UiFrameworkShould
{
    private readonly TestAppViewModel _testAppViewModel;
    private readonly RootController _root;
    private readonly ElementFactory _f;

    public UiFrameworkShould()
    {
        _testAppViewModel = new();
        _root = UiFactory.CreateRoot(_testAppViewModel, nameof(_testAppViewModel.Content));
        _f = new();
    }

    [Fact]
    public void Render_Text()
    {
        Render(CreateElement(Text, new { Text = "Some text" }));
        Root<TextViewModel>().Text.Should().Be("Some text");
    }

    [Fact]
    public void Render_Button()
    {
        var text = string.Empty;
        Render(CreateElement(Button, new { Text = "Click me", OnClick = new Action(() => text = "Button clicked") }));
        Root<ButtonViewModel>().Text.Should().Be("Click me");
        Root<ButtonViewModel>().Click.Execute(null);
        text.Should().Be("Button clicked");
    }

    [Fact]
    public void Render_Container()
    {
        Render(
            CreateElement(Container, null,
                CreateElement(Button, new { Text = "The button" }),
                CreateElement(Text, new { Text = "The text" })
            )
        );

        Root<ContainerViewModel>().Children[0].As<ButtonViewModel>().Text.Should().Be("The button");
        Root<ContainerViewModel>().Children[1].As<TextViewModel>().Text.Should().Be("The text");
    }

    [Fact]
    public void Render_Custom_Component()
    {
        Render(CreateElement(CustomComponent));

        Root<ContainerViewModel>()
            .Children[0].As<TextViewModel>().Text.Should().Be("default text");

        Root<ContainerViewModel>()
            .Children[1].As<ButtonViewModel>().Click.Execute(null);

        Root<ContainerViewModel>()
            .Children[0].As<TextViewModel>().Text.Should().Be("button clicked");

        return;

        Element CustomComponent(dynamic props, params Element[] children)
        {
            var (text, setText) = UseState("default text");
            return CreateElement(Container, null,
                CreateElement(Text, new { Text = text }),
                CreateElement(Button, new { OnClick = new Action(() => setText("button clicked")) })
            );
        }
    }

    private void Render(Element element)
    {
        _root.Render(element);
    }

    private T Root<T>()
    {
        return _testAppViewModel.Content.As<T>()!;
    }

    private Element CreateElement(
        Component element,
        dynamic? props = null,
        params Element[] children)
    {
        return _f.CreateElement(element, props, children);
    }
}
