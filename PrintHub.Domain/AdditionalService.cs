using PrintHub.Domain.Base;

namespace PrintHub.Domain;

public class AdditionalService : Identity
{
    public required string Name { get; set; }
    public required string Description { get; set; }

    public List<ServiceDetail>? ServiceDetails { get; set; }
}