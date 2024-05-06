using PrintHub.Domain.Base;

namespace PrintHub.Domain;

public class Material : Identity
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required decimal Price { get; set; }
    public required PrintingTechnology Technology { get; set; }
    public required List<Color> AvailableColors { get; set; } = null!;

    public virtual List<PrintingDetails>? PrintingDetails { get; set; }
}