using UiFramework.Elements;
using Xunit.Abstractions;

namespace UiFramework.JSX.Tests;

public class UseStateHookTests(ITestOutputHelper output)
{
    private readonly JsxViewEngineTestFixture _fixture = new();
    private JsxViewEngine ViewEngine =>
        _fixture.ViewEngine.SetDebugOutput(value => output.WriteLine(value.ToString()));

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
                          function ComponentA() {
                              const [state] = useState('state A');
                              return <Text text={state} />
                          }
                          
                          function ComponentB() {
                              const [state] = useState('state B');
                              return <Text text={state} />
                          }
                          
                          function StateComponent() {
                                const [showA, setShowA] = useState(true);
                                
                                return <Container>
                                    {showA && <ComponentA />}
                                    <ComponentB />
                                    <Button onClick={() => setShowA(false)} />
                               </Container>
                          }

                          <StateComponent />
                          """);

        ViewModel.Content.As<ContainerViewModel>().Children[^1].As<ButtonViewModel>().Click.Execute(null);
        ViewModel.Content.As<ContainerViewModel>().Children[0].As<TextViewModel>().Text.Should().Be("state B");
    }
}
