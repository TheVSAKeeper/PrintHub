namespace PrintHub.WPF.Shared;

public abstract class Checkable(bool isChecked)
{
    public bool IsChecked
    {
        get => isChecked;
        set
        {
            isChecked = value;
            OnCheckChanged?.Invoke();
        }
    }

    public event Action? OnCheckChanged;
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