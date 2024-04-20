using PrintHub.WPF.Shared.ViewModels;

namespace PrintHub.WPF.Endpoints.ColorEndpoints;

public class ColorFormViewModel : ViewModelBase
{
    private string _colorCode;
    private string _name;

    public string Name
    {
        get => _name;
        set => Set(ref _name, value);
    }

    public string ColorCode
    {
        get => _colorCode;
        set => Set(ref _colorCode, value);
    }
}