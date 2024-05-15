using PrintHub.Domain.Base;

namespace PrintHub.Domain;

public class Color : Identity
{
    public required string Name { get; set; }
    public required string ColorCode { get; set; }

    public virtual List<Material>? Materials { get; set; }
    public virtual List<PrintingDetails>? PrintingDetails { get; set; }
    public virtual List<Order>? Orders { get; set; }
}