using UiFramework.Primitives;

namespace UiFramework.JSX.Tests;

public class PrimitivesTests(JsxViewEngineTestFixture fixture) : IClassFixture<JsxViewEngineTestFixture>
{
    private Primitives ViewEngine { get; } = fixture.ViewEngine;
    private TestViewModel ViewModel { get; } = fixture.ViewModel;

    [Fact]
    public void Text_Test()
    {
        ViewEngine.Render("<Text text=\"Test text\"/>");
        ViewModel.Content.As<TextViewModel>().Text.Should().Be("Test text");
    }

    [Fact]
    public void Container_Test()
    {
        ViewEngine.Render("""
                          const items = [3, 4];
                          <Container>
                           <Text text="text 1" />
                           <Text text="text 2" />
                           {items.map(i => <Text text={'text ' + i} />)} 
                          </Container>
                          """);

        ViewModel.Content.As<ContainerViewModel>()
            .Children[0].As<TextViewModel>().Text.Should().Be("text 1");

        ViewModel.Content.As<ContainerViewModel>()
            .Children[1].As<TextViewModel>().Text.Should().Be("text 2");

        ViewModel.Content.As<ContainerViewModel>()
            .Children[2].As<TextViewModel>().Text.Should().Be("text 3");

        ViewModel.Content.As<ContainerViewModel>()
            .Children[3].As<TextViewModel>().Text.Should().Be("text 4");
    }

    [Fact]
    public void Children_As_Array_Test()
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
