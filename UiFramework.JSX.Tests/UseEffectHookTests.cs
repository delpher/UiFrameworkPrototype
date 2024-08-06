using UiFramework.JSX.Tests.Helpers;
using UiFramework.WPF;
using Xunit.Abstractions;

namespace UiFramework.JSX.Tests;

public class UseEffectHookTests(ITestOutputHelper output)
{
    private readonly JsxViewEngineTestFixture _fixture = new();
    private JsxViewEngine ViewEngine =>
        _fixture.ViewEngine.WithDebugOutput(value => output.WriteLine(value.ToString()));

    private TestViewModel ViewModel => _fixture.ViewModel;

    [Fact]
    public void Update_On_Dependency_Change()
    {
        List<int> log = [];

        ViewEngine.ExposeApi("TestLog", log);

        ViewEngine.Render("""
            function App() {
                const [version, setVersion] = useState(0);
                useEffect(() => TestLog.Add(version), [version]);
                
                return <Container>
                    <Button onClick={() => setVersion(0)} />
                    <Button onClick={() => setVersion(1)} />
                </Container>
            }
            
            <App />
        """);

        log.Should().HaveCount(1).And.Contain(0);

        ViewModel.Content.As<ContainerViewModel>().Children[0].As<ButtonViewModel>().Click.Execute(null);
        log.Should().HaveCount(1).And.Contain(0);

        ViewModel.Content.As<ContainerViewModel>().Children[0].As<ButtonViewModel>().Click.Execute(null);
        log.Should().HaveCount(1).And.Contain(0);

        ViewModel.Content.As<ContainerViewModel>().Children[1].As<ButtonViewModel>().Click.Execute(null);
        log.Should().HaveCount(2).And.Contain([0, 1]);

        ViewModel.Content.As<ContainerViewModel>().Children[1].As<ButtonViewModel>().Click.Execute(null);
        log.Should().HaveCount(2).And.Contain([0, 1]);

        ViewModel.Content.As<ContainerViewModel>().Children[0].As<ButtonViewModel>().Click.Execute(null);
        log.Should().HaveCount(3).And.Contain([0, 1, 0]);
    }
}
