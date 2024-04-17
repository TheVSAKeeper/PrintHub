using PrintHub.Domain.Base;

namespace PrintHub.Domain;

public class Material : Identity
{
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string Description { get; set; }

    public List<Color> AvailableColors { get; set; }
    public PrintingDetails PrintingDetails { get; set; }
}