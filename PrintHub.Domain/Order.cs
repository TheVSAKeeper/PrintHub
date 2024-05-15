using PrintHub.Domain.Base;

namespace PrintHub.Domain;

public class Order : Auditable
{
    public required string Description { get; set; }
    public required OrderStatus Status { get; set; }

    public required Guid ClientId { get; set; }
    public Client? Client { get; set; }

    public List<Color>? RequiredColors { get; set; }

    public virtual List<ServiceDetail>? ServiceDetails { get; set; }
    public virtual List<Sample>? Samples { get; set; }
    public virtual List<Item>? Items { get; set; }
}