using PrintHub.WPF.Endpoints.ColorEndpoints;
using PrintHub.WPF.Shared.ViewModels;

namespace PrintHub.WPF.Endpoints.MaterialEndpoints;

public class MaterialFormViewModel : ViewModelBase
{
    private decimal _price;

    private List<ColorViewModel> _availableColors;

    private string _description;
    private string _name;

    public string Name
    {
        get => _name;
        set => Set(ref _name, value);
    }

    public string Description
    {
        get => _description;
        set => Set(ref _description, value);
    }

    public decimal Price
    {
        get => _price;
        set => Set(ref _price, value);
    }

    public List<ColorViewModel> AvailableColors
    {
        get => _availableColors;
        set => Set(ref _availableColors, value);
    }
}