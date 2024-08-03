using System.Windows.Input;

namespace UiFramework.Primitives;

public class ButtonViewModel(Action? onClick)
{
    public string? Text { get; init; } = string.Empty;
    public ICommand Click { get; } = new ActionCommand(onClick);

    private class ActionCommand(Action? action) : ICommand
    {
        public bool CanExecute(object? parameter) =>
            action != null;

        public void Execute(object? parameter) =>
            action?.Invoke();

        public event EventHandler? CanExecuteChanged;
    }
}
