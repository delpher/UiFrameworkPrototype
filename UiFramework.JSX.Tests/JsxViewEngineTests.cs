using System.ComponentModel;
using UiFramework.Elements;

namespace UiFramework.JSX.Tests;

public class JsxViewEngineShould
{
    private readonly TestViewModel _viewModel;
    private readonly JsxViewEngine _viewEngine;

    public JsxViewEngineShould()
    {
        _viewModel = new();
        _viewEngine = new(UiFactory.CreateRoot(_viewModel, nameof(_viewModel.Content)));
    }

    [Fact]
    public void Standard_Element_Test()
    {
        _viewEngine.Render("<Text text=\"Test text\"/>");
        _viewModel.Content.As<TextViewModel>().Text.Should().Be("Test text");
    }

    [Fact]
    public void Custom_Component_Test()
    {
        _viewEngine.Render("""
                           function CustomComponent() {
                              return <Text text="Test text"/>
                           }
                           <CustomComponent />
                           """);
        _viewModel.Content.As<TextViewModel>().Text.Should().Be("Test text");
    }

    [Fact]
    public void Child_Elements_Test()
    {
        _viewEngine.Render("""
                           <Container>
                            <Text text="text 1" />
                            <Text text="text 2" />
                           </Container>
                           """);
        _viewModel.Content.As<ContainerViewModel>()
            .Children[0].As<TextViewModel>().Text.Should().Be("text 1");
        _viewModel.Content.As<ContainerViewModel>()
            .Children[1].As<TextViewModel>().Text.Should().Be("text 2");
    }

    [Fact]
    public void A_Child_Is_Array_Test()
    {
        _viewEngine.Render("""
                           const items = ['one', 'two', 'three'];
                           <Container>
                            {items.map(item => <Text text={item} />)}
                           </Container>
                           """);

        _viewModel.Content.As<ContainerViewModel>()
            .Children[0].As<TextViewModel>().Text.Should().Be("one");
        _viewModel.Content.As<ContainerViewModel>()
            .Children[1].As<TextViewModel>().Text.Should().Be("two");
        _viewModel.Content.As<ContainerViewModel>()
            .Children[2].As<TextViewModel>().Text.Should().Be("three");
    }

    [Fact]
    public void UseState_Hook_Test()
    {
        _viewEngine.Render("""
                           function StateComponent() {
                                const [text, setText] = useState("initial text");
                                
                                return <Container>
                                    <Text text={text} />
                                    <Button onClick={() => setText("button clicked")} />
                                </Container>
                           }
                           
                           <StateComponent />
                           """);
        _viewModel.Content.As<ContainerViewModel>().Children[0].As<TextViewModel>().Text.Should().Be("initial text");
        _viewModel.Content.As<ContainerViewModel>().Children[1].As<ButtonViewModel>().Click.Execute(null);
        _viewModel.Content.As<ContainerViewModel>().Children[0].As<TextViewModel>().Text.Should().Be("button clicked");
    }
}

public class TestViewModel
{
    public object? Content { get; set; }
}
