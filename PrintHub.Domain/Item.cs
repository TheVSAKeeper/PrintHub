using PrintHub.Domain.Base;

namespace PrintHub.Domain;

public class Item : Auditable
{
    public required string Description { get; set; }
    public required decimal Weight { get; set; }
    public required decimal DevelopmentCost { get; set; }
    public required bool Ready { get; set; }

    public required Guid OrderId { get; set; }
    public Order? Order { get; set; }

    public required Guid PrintingDetailsId { get; set; }
    public PrintingDetails? PrintingDetails { get; set; }
}