namespace UiFramework.JSX.Tests;

public class BabelTransformShould
{
    [Fact]
    public void TransformStandaloneJsx()
    {
        new Babel()
            .Transform("<div/>")
            .Should()
            .Be("\"use strict\";\n\n/*#__PURE__*/React.createElement(\"div\", null);");
    }
}
