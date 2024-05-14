using PrintHub.WPF.Endpoints.ServiceDetailEndpoints.ViewModels;

namespace PrintHub.WPF.Endpoints.AdditionalServiceEndpoints.ViewModels;

public class AdditionalServiceViewModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }

    public List<ServiceDetailViewModel>? ServiceDetails { get; set; }
}