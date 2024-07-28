using UiFramework.Elements;

namespace UiFramework.JSX.Tests;

public class JsxViewEngineShould
{
    private readonly TestViewModel _viewModel;
    private readonly JsxViewEngine _viewEngine;

    public JsxViewEngineShould()
    {
        _viewModel = new();
        _viewEngine = new(ViewFactory.CreateRoot(_viewModel, nameof(_viewModel.Content)));
    }

    [Fact]
    public void Standard_Element_Test()
    {
        _viewEngine.Render("<Text Text=\"Test text\"/>");
        _viewModel.Content.As<TextViewModel>().Text.Should().Be("Test text");
    }

    [Fact]
    public void Custom_Component_Test()
    {
        _viewEngine.Render("""
                           function CustomComponent() {
                              return <Text Text="Test text"/>
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
                            <Text Text="text 1" />
                            <Text Text="text 2" />
                           </Container>
                           """);
        _viewModel.Content.As<ContainerViewModel>()
            .Children[0].As<TextViewModel>().Text.Should().Be("text 1");
        _viewModel.Content.As<ContainerViewModel>()
            .Children[1].As<TextViewModel>().Text.Should().Be("text 2");
    }

    [Fact]
    public void UseState_Hook_Test()
    {
        _viewEngine.Render("""
                           function StateComponent() {
                                const state = useState("initial text");
                                
                                return <Container>
                                    <Text Text={state.text} />
                                    <Button onClick={() => state.setText("button clicked")} />
                                </Container>
                           }
                           
                           (<StateComponent />)
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
