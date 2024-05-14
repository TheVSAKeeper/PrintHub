using PrintHub.Domain.Base;

namespace PrintHub.Domain;

public class ServiceDetail : Identity
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required decimal Price { get; set; }

    public Guid AdditionalServiceId { get; set; }
    public AdditionalService? AdditionalService { get; set; }

    public virtual List<Order>? Orders { get; set; }
}