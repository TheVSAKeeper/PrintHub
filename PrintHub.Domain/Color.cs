using PrintHub.Domain.Base;

namespace PrintHub.Domain;

public class Color : Identity
{
    public string Name { get; set; }
    public string ColorCode { get; set; }

    public Guid MaterialId { get; set; }
    public Material Material { get; set; }
}