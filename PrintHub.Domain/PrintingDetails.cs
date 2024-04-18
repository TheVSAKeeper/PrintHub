using PrintHub.Domain.Base;

namespace PrintHub.Domain;

public class PrintingDetails : Identity
{
    public required PrintingTechnology Technology { get; set; }

    public required Guid ColorId { get; set; }
    public Color? Color { get; set; }

    public required Guid MaterialId { get; set; }
    public Material? Material { get; set; }

    public virtual List<Item>? Items { get; set; }
    public virtual List<Sample>? Samples { get; set; }
}