using PrintHub.WPF.Endpoints.ColorEndpoints.ViewModels;

namespace PrintHub.WPF.Endpoints.MaterialEndpoints.ViewModels;

public class MaterialViewModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public PrintingTechnology Technology { get; set; }
    public List<ColorViewModel> AvailableColors { get; set; }
}