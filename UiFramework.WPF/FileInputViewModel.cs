using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Microsoft.Win32;

namespace UiFramework.WPF;

public sealed class FileInputViewModel(Action<string> onChange) : INotifyPropertyChanged
{
    private string? _selectedFile;
    public ICommand Select => new ActionCommand(ShowFileDialog);

    public string? SelectedFile
    {
        get => _selectedFile;
        set => SetField(ref _selectedFile, value);
    }

    private void ShowFileDialog()
    {
        var fileDialog = new OpenFileDialog();
        if (fileDialog.ShowDialog() != true) return;
        SelectedFile = fileDialog.FileName;
        onChange(fileDialog.FileName);
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null) =>
        PropertyChanged?.Invoke(this, new(propertyName));

    private void SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return;
        field = value;
        OnPropertyChanged(propertyName);
    }
}
