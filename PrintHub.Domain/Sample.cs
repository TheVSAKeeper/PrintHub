using PrintHub.Domain.Base;

namespace PrintHub.Domain;

public class Sample : Auditable
{
    public required string Description { get; set; }
    public required bool Approved { get; set; }

    public required Guid OrderId { get; set; }
    public Order? Order { get; set; }

    public required Guid PrintingDetailsId { get; set; }
    public PrintingDetails? PrintingDetails { get; set; }
}