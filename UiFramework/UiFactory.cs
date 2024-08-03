namespace UiFramework;

public static class UiFactory
{
    public static RootController CreateRoot(object viewModel, string propertyName) =>
        new(content => viewModel.GetType().GetProperty(propertyName)!.SetValue(viewModel, content));
}
