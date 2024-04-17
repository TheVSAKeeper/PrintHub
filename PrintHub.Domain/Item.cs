using PrintHub.Domain.Base;

namespace PrintHub.Domain;

public class Item : Auditable
{
    public string Description { get; set; }
    
    public Guid PrintingDetailsId { get; set; }
    public PrintingDetails PrintingDetails { get; set; }
    
    public bool Ready { get; set; }

    public Guid OrderId { get; set; }
    public Order Order { get; set; }
}