using PrintHub.Domain;

namespace PrintHub.WPF.Endpoints.OrderEndpoints.ViewModels;

public class OrderViewModel
{
    public Guid Id { get; set; }

    public required string Description { get; set; }
    public required OrderStatus Status { get; set; }

    public required Guid ClientId { get; set; }
    public Client? Client { get; set; }

    public List<Color>? RequiredColors { get; set; }

    public virtual List<Sample>? Samples { get; set; }
    public virtual List<Item>? Items { get; set; }

    public override string ToString() => $"{nameof(Id)}: {Id}, {nameof(Description)}: {Description}, {nameof(Status)}: {Status}, {nameof(ClientId)}: {ClientId}, {nameof(Client)}: {Client}, {nameof(RequiredColors)}: {RequiredColors}, {nameof(Samples)}: {Samples}, {nameof(Items)}: {Items}";
}