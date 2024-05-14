using PrintHub.WPF.Endpoints.AdditionalServiceEndpoints.ViewModels;

namespace PrintHub.WPF.Endpoints.ServiceDetailEndpoints.ViewModels;

public class ServiceDetailViewModel
{
    public Guid Id { get; set; }

    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }

    public AdditionalServiceViewModel? AdditionalService { get; set; }
}