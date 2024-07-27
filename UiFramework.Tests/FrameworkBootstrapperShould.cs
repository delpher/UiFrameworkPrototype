using FluentAssertions;
using Microsoft.ClearScript.Windows;

namespace UiFramework.Tests;

public class FrameworkBootstrapperShould
{
    [Fact]
    public void RunFramework()
    {
        var eng = new JScriptEngine("test");
        var result = eng.Evaluate("1 + 3");
        result.Should().Be(4);
    }
}
