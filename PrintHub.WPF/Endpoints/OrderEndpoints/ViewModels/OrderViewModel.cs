using PrintHub.Domain;
using PrintHub.WPF.Endpoints.ColorEndpoints.ViewModels;

namespace PrintHub.WPF.Endpoints.OrderEndpoints.ViewModels;

public class OrderViewModel
{
    public Guid Id { get; set; }

    public required string Description { get; set; }
    public required OrderStatus Status { get; set; }

    public DateTime UpdatedAt { get; set; }
    public string UpdatedBy { get; set; }

    public required Guid ClientId { get; set; }
    public Client? Client { get; set; }

    public List<ColorViewModel>? RequiredColors { get; set; }

    public virtual List<Sample>? Samples { get; set; }
    public virtual List<Item>? Items { get; set; }

    public override string ToString() => $"Description: {Description} "
                                         + $"Status: {Status} "
                                         + $"Updated: {UpdatedAt:g} "
                                         + $"By: {UpdatedBy} "
                                         + $"Client: {Client?.Email} "
                                         + $"Required colors: {string.Join(", ", RequiredColors ?? [])}"
                                         + $"Samples: {Samples?.Count} "
                                         + $"Items: {Items?.Count}";
}