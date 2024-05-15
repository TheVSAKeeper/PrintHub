using PrintHub.WPF.Endpoints.ItemEndpoints.ViewModels;

namespace PrintHub.WPF.Endpoints.OrderEndpoints.ViewModels;

public class OrderUpdateViewModel
{
    public Guid Id { get; set; }
    public string Description { get; set; }

    public OrderStatus Status { get; set; }
    public List<ItemViewModel>? Items { get; set; }
}