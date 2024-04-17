using PrintHub.Domain.Base;

namespace PrintHub.Domain;

public class PrintingDetails : Identity
{
    public PrintingTechnology Technology { get; set; }

    public int MaterialId { get; set; }
    public Material Material { get; set; }
}