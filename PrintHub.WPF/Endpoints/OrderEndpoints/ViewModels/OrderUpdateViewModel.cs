using PrintHub.Domain;

namespace PrintHub.WPF.Endpoints.OrderEndpoints.ViewModels;

public class OrderUpdateViewModel
{
    public Guid Id { get; set; }
    public string Description { get; set; }
    public List<Color>? RequiredColors { get; set; }
}