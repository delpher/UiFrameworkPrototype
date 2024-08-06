using UiFramework.JSX.Tests.Helpers;
using UiFramework.WPF;
using Xunit.Abstractions;

namespace UiFramework.JSX.Tests;

public class UseContextHookTests(ITestOutputHelper output)
{
    private readonly JsxViewEngineTestFixture _fixture = new();
    private JsxViewEngine ViewEngine =>
        _fixture.ViewEngine.WithDebugOutput(value => output.WriteLine(value.ToString()));

    private TestViewModel ViewModel => _fixture.ViewModel;

    [Fact]
    public void Update_On_Provider_Value_Change()
    {
        ViewEngine.Render("""
                            const TestContext = createContext();
                            
                            function TestContextConsumer() {
                                const value = useContext(TestContext);
                                return <Text text={value} />
                            }                          
                            
                            function App() {
                                const [state, setState] = useState("initial state");
                                
                                return <TestContext.Provider value={state}>
                                    <Container>
                                        <TestContextConsumer />
                                        <Button onClick={() => setState("updated state")} />
                                    </Container>
                                </TestContext.Provider>
                            }
                            
                            <App />
                          """);

        ViewModel.Content.As<ContainerViewModel>()
            .Children[0].As<TextViewModel>().Text.Should().Be("initial state");

        ViewModel.Content.As<ContainerViewModel>()
            .Children[1].As<ButtonViewModel>().Click.Execute(null);

        ViewModel.Content.As<ContainerViewModel>()
            .Children[0].As<TextViewModel>().Text.Should().Be("updated state");
    }
}
