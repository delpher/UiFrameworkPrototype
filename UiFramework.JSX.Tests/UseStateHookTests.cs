using UiFramework.JSX.Tests.Helpers;
using UiFramework.Primitives;
using Xunit.Abstractions;

namespace UiFramework.JSX.Tests;

public class UseStateHookTests(ITestOutputHelper output)
{
    private readonly JsxViewEngineTestFixture _fixture = new();
    private JsxViewEngine ViewEngine =>
        _fixture.ViewEngine.WithDebugOutput(value => output.WriteLine(value.ToString()));

    private TestViewModel ViewModel => _fixture.ViewModel;

    [Fact]
    public void Single_State_Test()
    {
        ViewEngine.Render("""
                          function StateComponent() {
                               const [text, setText] = useState("initial text");

                               return <Container>
                                   <Text text={text} />
                                   <Button onClick={() => setText("button clicked")} />
                               </Container>
                          }

                          <StateComponent />
                          """);

        ViewModel.Content.As<ContainerViewModel>().Children[0].As<TextViewModel>().Text.Should().Be("initial text");
        ViewModel.Content.As<ContainerViewModel>().Children[1].As<ButtonViewModel>().Click.Execute(null);
        ViewModel.Content.As<ContainerViewModel>().Children[0].As<TextViewModel>().Text.Should().Be("button clicked");
    }

    [Fact]
    public void Elements_List()
    {
        ViewEngine.Render("""
                          function StateComponent() {
                                const [items, setItems] = useState([]);
                                
                                return <Container>
                                    {items.map(i => <Text text={i} />)}
                                    <Button onClick={() => setItems([...items, items.length])} />
                               </Container>
                          }

                          <StateComponent />
                          """);

        ViewModel.Content.As<ContainerViewModel>().Children[^1].As<ButtonViewModel>().Click.Execute(null);
        ViewModel.Content.As<ContainerViewModel>().Children[0].As<TextViewModel>().Text.Should().Be("0");

        ViewModel.Content.As<ContainerViewModel>().Children[^1].As<ButtonViewModel>().Click.Execute(null);
        ViewModel.Content.As<ContainerViewModel>().Children[0].As<TextViewModel>().Text.Should().Be("0");
        ViewModel.Content.As<ContainerViewModel>().Children[1].As<TextViewModel>().Text.Should().Be("1");
    }

    [Fact]
    public void State_Management_Test()
    {
        ViewEngine.Render("""
                          function Component({name}) {
                              const [state] = useState(name);
                              return <Text text={state} />
                          }
                          
                          function StateComponent() {
                                const [showA, setShowA] = useState(true);
                                
                                return <Container>
                                    {showA && <Component name="A" />}
                                    <Component name="B" />
                                    <Button onClick={() => setShowA(false)} />
                               </Container>
                          }

                          <StateComponent />
                          """);

        ViewModel.Content.As<ContainerViewModel>().Children[^1].As<ButtonViewModel>().Click.Execute(null);
        ViewModel.Content.As<ContainerViewModel>().Children[0].As<TextViewModel>().Text.Should().Be("B");
    }

    [Fact]
    public void State_Cleanup_Test()
    {
        ViewEngine.ExposeApi("crypto", new { generateUUID = new Func<string>(() => Guid.NewGuid().ToString())});

        ViewEngine.Render("""
                          function Component() {
                              const [state] = useState(crypto.generateUUID());
                              return <Text text={state} />
                          }

                          function App() {
                                const [show, setShow] = useState(true);
                                
                                return <Container>
                                    {show && <Component />}
                                    <Button onClick={() => setShow(!show)} />
                               </Container>
                          }

                          <App />
                          """);

        var initialText = ViewModel.Content.As<ContainerViewModel>().Children[0].As<TextViewModel>().Text;
        ViewModel.Content.As<ContainerViewModel>().Children[^1].As<ButtonViewModel>().Click.Execute(null);
        ViewModel.Content.As<ContainerViewModel>().Children[^1].As<ButtonViewModel>().Click.Execute(null);
        ViewModel.Content.As<ContainerViewModel>().Children[0].As<TextViewModel>().Text.Should().NotBe(initialText);

    }
}
