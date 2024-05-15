using PrintHub.Domain.Base;
using PrintHub.WPF.Endpoints.PrintingDetailsEndpoints.ViewModels;

namespace PrintHub.WPF.Endpoints.ItemEndpoints.ViewModels;

public class ItemViewModel : IAuditable
{
    public Guid Id { get; set; }

    public string Description { get; set; }
    public bool Ready { get; set; }
    public decimal Weight { get; set; }
    public decimal DevelopmentCost { get; set; }
    public Guid OrderId { get; set; }

    public Guid PrintingDetailsId { get; set; }
    public PrintingDetailsViewModel PrintingDetails { get; set; }
    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string UpdatedBy { get; set; }
}