namespace UiFramework;

public static class UiFactory
{
    public static RootController CreateRoot(object viewModel, string propertyName)
    {
        var propertyDescriptor = viewModel.GetType().GetProperty(propertyName);
        return new(content => propertyDescriptor!.SetValue(viewModel, content));
    }
}
