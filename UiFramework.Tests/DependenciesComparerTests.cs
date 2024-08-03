namespace UiFramework.Tests;

public class DependenciesComparerTests
{
    [Fact]
    public void Should_Be_Different_When_Left_Both_Are_Null() =>
        DependenciesComparer.AreSame(null, null)
            .Should().BeFalse();

    [Fact]
    public void Should_Be_Different_When_Left_Is_Null() =>
        DependenciesComparer.AreSame(null, [])
            .Should().BeFalse();

    [Fact]
    public void Should_Be_Different_When_Right_Is_Null() =>
        DependenciesComparer.AreSame(null, [])
            .Should().BeFalse();

    [Fact]
    public void Empty_Arrays_Should_Be_Different() =>
        DependenciesComparer.AreSame([], [])
            .Should().BeFalse();

    [Fact]
    public void Should_Throw_When_Arrays_Have_Different_Length() =>
        new Action(() => DependenciesComparer.AreSame([1], []))
            .Should().Throw<InvalidOperationException>();

    [Theory]
    [InlineData(null, null, true)]
    [InlineData(null, 1, false)]
    [InlineData(1, null, false)]
    [InlineData(1, 1, true)]
    [InlineData("one", 1, false)]
    [InlineData(1, "one", false)]
    [InlineData("one", "one", true)]
    public void Single_Element_Tests(object? first, object? second, bool areSame) =>
        DependenciesComparer.AreSame([first], [second])
            .Should().Be(areSame);

    [Theory]
    [InlineData((object?[]?) [null, null], (object?[]?) [null, null], true)]
    [InlineData((object?[]?) ["one", null], (object?[]?) [null, "one"], false)]
    [InlineData((object?[]?) [null, null], (object?[]?) ["one", "one"], false)]
    [InlineData((object?[]?) ["one", "one"], (object?[]?) [null, null], false)]
    [InlineData((object?[]?) ["one", null], (object?[]?) ["one", "one"], false)]
    [InlineData((object?[]?) ["one", "one"], (object?[]?) ["one", null], false)]
    [InlineData((object?[]?) ["one", "one"], (object?[]?) ["one", "one"], true)]
    public void TwoElementCases(object?[]? first, object?[]? second, bool areSame) =>
        DependenciesComparer.AreSame(first, second)
            .Should().Be(areSame);

    [Fact]
    public void ThreeElementCase() =>
        DependenciesComparer.AreSame([1, "two", 3], [1, "two", 3]);

    [Fact]
    public void Deep_Equality_Test() =>
        DependenciesComparer.AreSame([
            new TestStruct { Id = 1, Name = "first" },
            new TestStruct { Id = 2, Name = "second" },
        ], [
            new TestStruct { Id = 1, Name = "first" },
            new TestStruct { Id = 2, Name = "second" },
        ]).Should().BeTrue();

    private struct TestStruct
    {
        public string Name { get; set; }
        public int Id { get; set; }
    }
}
