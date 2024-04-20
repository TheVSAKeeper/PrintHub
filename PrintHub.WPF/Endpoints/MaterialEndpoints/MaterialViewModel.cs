using PrintHub.WPF.Endpoints.ColorEndpoints;

namespace PrintHub.WPF.Endpoints.MaterialEndpoints;

public class MaterialViewModel
{
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public List<ColorViewModel> AvailableColors { get; set; }
}