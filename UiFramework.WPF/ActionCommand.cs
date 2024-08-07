using System.Windows.Input;

namespace UiFramework.WPF;

public class ActionCommand(Action? action) : ICommand
{
    public bool CanExecute(object? parameter) =>
        action != null;

    public void Execute(object? parameter) =>
        action?.Invoke();

    public event EventHandler? CanExecuteChanged;
}