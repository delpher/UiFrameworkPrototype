using System.Windows.Input;

namespace UiFramework.WPF;

public class ButtonViewModel(Action? onClick)
{
    public string? Text { get; init; } = string.Empty;
    public ICommand Click { get; } = new ActionCommand(onClick);
}
