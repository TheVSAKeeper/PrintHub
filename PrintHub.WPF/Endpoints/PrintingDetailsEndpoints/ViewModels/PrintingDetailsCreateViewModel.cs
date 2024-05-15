namespace PrintHub.WPF.Endpoints.PrintingDetailsEndpoints.ViewModels;

public class PrintingDetailsCreateViewModel
{
    public Guid Id { get; set; }
    public required string Description { get; set; }
    public required PrintingTechnology Technology { get; set; }
    public required Guid ColorId { get; set; }
    public required Guid MaterialId { get; set; }
}