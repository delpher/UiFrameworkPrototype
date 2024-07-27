using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace UiFramework.SampleApp;

public class MainWindowViewModel : INotifyPropertyChanged
{
    private object _content;

    public object Content
    {
        get => _content;
        set => SetField(ref _content, value);
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new(propertyName));
    }

    private bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}