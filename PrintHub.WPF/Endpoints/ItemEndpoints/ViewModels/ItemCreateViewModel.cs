namespace PrintHub.WPF.Endpoints.ItemEndpoints.ViewModels;

public class ItemCreateViewModel
{
    public Guid Id { get; set; }
    public required string Description { get; set; }
    public required decimal Weight { get; set; }
    public required decimal DevelopmentCost { get; set; }
    public required Guid OrderId { get; set; }
    public required Guid PrintingDetailsId { get; set; }
}