using PrintHub.WPF.Endpoints.ServiceDetailEndpoints.ViewModels;
using PrintHub.WPF.Shared;

namespace PrintHub.WPF.Endpoints.AdditionalServiceEndpoints.ViewModels;

public class AdditionalServiceSelectViewModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }

    public List<Checkable<ServiceDetailViewModel>>? ServiceDetails { get; set; }
}