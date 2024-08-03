using FluentAssertions;
using static UiFramework.Framework;

namespace UiFramework.Tests;

public class RootControllerShould
{
    private readonly TestOutput _output;
    private readonly RootController _root;

    private static Primitive TestPrimitive =>
        (props, children) =>
            () => new TestViewModel
            {
                Children = children.Select(c => c()).ToArray(),
                Props = props
            };

    public RootControllerShould()
    {
        _output = new();
        _root = new(_output.Output);
    }

    [Fact]
    public void Render_Primitive()
    {
        _root.Render(CreateElement(TestPrimitive));
        _output.ViewModel.Should().BeOfType<TestViewModel>();
    }

    [Fact]
    public void Render_Props()
    {
        var expectedProps = new Dictionary<string, object?>
        {
            {"TestProp", "test_value"}
        };
        _root.Render(CreateElement(TestPrimitive, expectedProps));
        _output.ViewModel!.Props!["TestProp"].Should().Be("test_value");
    }

    [Fact]
    public void Render_Children()
    {
        _root.Render(CreateElement(
            TestPrimitive,
            null,
            CreateElement(TestPrimitive),
            CreateElement(TestPrimitive)
        ));

        _output.ViewModel!.Children.Should()
            .HaveCount(2)
            .And.AllBeOfType<TestViewModel>();
    }

    [Fact]
    public void Render_Deep_Nested_Children()
    {
        _root.Render(CreateElement(
            TestPrimitive,
            null,
            CreateElement(TestPrimitive,
                null,
                CreateElement(TestPrimitive),
                CreateElement(TestPrimitive)
            ),
            CreateElement(TestPrimitive,
                null,
                CreateElement(TestPrimitive))
        ));

        _output.ViewModel!.Children.Should()
            .HaveCount(2)
            .And.SatisfyRespectively(
                first => first.As<TestViewModel>().Children
                    .Should().HaveCount(2).And.AllBeOfType<TestViewModel>(),
                second => second.As<TestViewModel>().Children
                    .Should().HaveCount(1).And.AllBeOfType<TestViewModel>()
            );
    }

    [Fact]
    public void Render_Component()
    {
        IDictionary<string,object?> props = new Dictionary<string, object?>();
        ElementFactory[] children = [];

        _root.Render(CreateElement(FunctionComponent, props, children));

        _output.ViewModel.Should().NotBeNull();
        return;

        ElementFactory FunctionComponent(IDictionary<string, object?>? p, params ElementFactory?[] c) =>
            CreateElement(TestPrimitive);
    }

    private class TestOutput
    {
        public TestViewModel? ViewModel { get; private set; }

        public void Output(object? viewModel) =>
            ViewModel = viewModel as TestViewModel;
    }

    private class TestViewModel
    {
        public object?[]? Children { get; init; }
        public IDictionary<string,object?>? Props { get; init; }
    }
}
