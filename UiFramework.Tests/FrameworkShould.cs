using UiFramework.Primitives;
using UiFramework.Tests.Helpers;
using static UiFramework.Primitives.Elements;
using static UiFramework.Framework;

namespace UiFramework.Tests;

public class FrameworkShould
{
    private readonly TestAppViewModel _testAppViewModel;
    private readonly RootController _root;

    public FrameworkShould()
    {
        _testAppViewModel = new();
        _root = UiFactory.CreateRoot(_testAppViewModel, nameof(_testAppViewModel.Content));
    }

    private void Render(ElementFactory element) => _root.Render(element);
    private T Root<T>() => _testAppViewModel.Content.As<T>()!;

    [Fact]
    public void Render_Text()
    {
        Render(CreateElement(Text, new { text = "Some text" }));
        Root<TextViewModel>().Text.Should().Be("Some text");
    }

    [Fact]
    public void Render_Button()
    {
        var text = string.Empty;

        Render(CreateElement(Button,
            new { text = "Click me", onClick = new Action(() => text = "Button clicked") }));

        Root<ButtonViewModel>().Text.Should().Be("Click me");
        Root<ButtonViewModel>().Click.Execute(null);
        text.Should().Be("Button clicked");
    }

    [Fact]
    public void Render_Container()
    {
        Render(
            CreateElement(Container, null, [
                CreateElement(Button, new { text = "The button" }),
                CreateElement(Text, new { text = "The text" })
            ])
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

        ElementFactory CustomComponent(dynamic props, params ElementFactory?[] children)
        {
            var (text, setText) = UseState("default text");
            return CreateElement(Container, null,
                CreateElement(Text, new { text }),
                CreateElement(Button, new { onClick = new Action(() => setText("button clicked")) })
            );
        }
    }

    [Fact]
    public void Render_Fragment()
    {
        Render(CreateElement(Fragment, null,
            CreateElement(Text, new {text = "child 1"}),
            CreateElement(Text, new {text = "child 2"})
        ));

        Root<object[]>()
            .Should().SatisfyRespectively(
                first => first.As<TextViewModel>().Text.Should().Be("child 1"),
                second => second.As<TextViewModel>().Text.Should().Be("child 2")
            );
    }

    [Fact]
    public void Render_Fragment_With_Single_Child()
    {
        Render(CreateElement(Fragment, null,
            CreateElement(Text, new {text = "single child"})
        ));

        Root<TextViewModel>().Text.Should().Be("single child");
    }
}
