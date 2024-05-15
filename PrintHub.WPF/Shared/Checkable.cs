using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PrintHub.WPF.Shared;

public abstract class Checkable(bool isChecked) : INotifyPropertyChanged
{
    public bool IsChecked
    {
        get => isChecked;
        set
        {
            SetField(ref isChecked, value);
            OnCheckChanged?.Invoke();
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public event Action? OnCheckChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value))
            return false;

        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}

public class Checkable<T>
{
    private bool _isChecked;

    public required T ViewModel { get; set; }

    public bool IsChecked
    {
        get => _isChecked;
        set
        {
            _isChecked = value;
            OnCheckChanged?.Invoke();
        }
    }

    public event Action? OnCheckChanged;
}