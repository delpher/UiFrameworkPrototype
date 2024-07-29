using UiFramework.Elements;

namespace UiFramework.JSX.Tests;

public class JsxViewEngineTests(JsxViewEngineTestFixture fixture) : IClassFixture<JsxViewEngineTestFixture>
{
    private JsxViewEngine ViewEngine { get; } = fixture.ViewEngine;
    private TestViewModel ViewModel { get; } = fixture.ViewModel;

    [Fact]
    public void Standard_Element_Test()
    {
        ViewEngine.Render("<Text text=\"Test text\"/>");
        ViewModel.Content.As<TextViewModel>().Text.Should().Be("Test text");
    }

    [Fact]
    public void Custom_Component_Test()
    {
        ViewEngine.Render("""
                           function CustomComponent() {
                              return <Text text="Test text"/>
                           }
                           <CustomComponent />
                           """);
        ViewModel.Content.As<TextViewModel>().Text.Should().Be("Test text");
    }

    [Fact]
    public void Child_Elements_Test()
    {
        ViewEngine.Render("""
                           <Container>
                            <Text text="text 1" />
                            <Text text="text 2" />
                           </Container>
                           """);

        ViewModel.Content.As<ContainerViewModel>()
            .Children[0].As<TextViewModel>().Text.Should().Be("text 1");

        ViewModel.Content.As<ContainerViewModel>()
            .Children[1].As<TextViewModel>().Text.Should().Be("text 2");
    }

    [Fact]
    public void A_Child_Is_Array_Test()
    {
        ViewEngine.Render("""
                           const items = ['one', 'two', 'three'];
                           <Container>
                            {items.map(item => <Text text={item} />)}
                           </Container>
                           """);

        ViewModel.Content.As<ContainerViewModel>()
            .Children[0].As<TextViewModel>().Text.Should().Be("one");
        ViewModel.Content.As<ContainerViewModel>()
            .Children[1].As<TextViewModel>().Text.Should().Be("two");
        ViewModel.Content.As<ContainerViewModel>()
            .Children[2].As<TextViewModel>().Text.Should().Be("three");
    }
}
