using PrintHub.Domain.Base;

namespace PrintHub.Domain;

public class Sample : Auditable
{
    public string Description { get; set; }
    public PrintingTechnology PrintingTechnology { get; set; }
    public Material Material { get; set; }
    public bool Approved { get; set; }

    public int OrderId { get; set; }
    public Order Order { get; set; }
}