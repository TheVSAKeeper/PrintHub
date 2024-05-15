using PrintHub.WPF.Endpoints.ColorEndpoints.ViewModels;
using PrintHub.WPF.Endpoints.ServiceDetailEndpoints.ViewModels;

namespace PrintHub.WPF.Endpoints.OrderEndpoints.ViewModels;

public class OrderCreateViewModel
{
    public Guid Id { get; set; }
    public string Description { get; set; }
    public Guid ClientId { get; set; }
    public List<ColorViewModel> RequiredColors { get; set; }
    public List<ServiceDetailViewModel>? ServiceDetails { get; set; }
}