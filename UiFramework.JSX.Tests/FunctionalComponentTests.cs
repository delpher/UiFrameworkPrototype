using UiFramework.JSX.Tests.Helpers;
using UiFramework.WPF;
using Xunit.Abstractions;

namespace UiFramework.JSX.Tests;

public class FunctionalComponentTests(ITestOutputHelper output)
{
    private readonly JsxViewEngineTestFixture _fixture = new();
    private JsxViewEngine ViewEngine =>
        _fixture.ViewEngine.WithDebugOutput(value => output.WriteLine(value.ToString()));

    private TestViewModel ViewModel => _fixture.ViewModel;

    [Fact]
    public void Render_Test()
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
    public void Passing_Props_Test()
    {
        ViewEngine.Render("""
                          function CustomComponent(props) {
                             return <Text text={props.title} />
                          }
                          <CustomComponent title="Test text" />
                          """);
        ViewModel.Content.As<TextViewModel>().Text.Should().Be("Test text");
    }

    [Fact]
    public void Passing_Children_Test()
    {
        ViewEngine.Render("""
                          function CustomComponent(props, children) {
                             return <Container>{children}</Container>
                          }
                          <CustomComponent>
                            <Text text="first" />
                            <Text text="second" />
                          </CustomComponent>
                          """);

        ViewModel.Content.As<ContainerViewModel>()
            .Children.Should().HaveCount(2)
            .And.SatisfyRespectively(
                first => first.As<TextViewModel>().Text.Should().Be("first"),
                second => second.As<TextViewModel>().Text.Should().Be("second")
            );
    }
}
