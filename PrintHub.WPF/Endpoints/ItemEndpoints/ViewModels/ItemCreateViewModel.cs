namespace PrintHub.WPF.Endpoints.ItemEndpoints.ViewModels;

public class ItemCreateViewModel
{
    public Guid Id { get; set; }
    public string Description { get; set; }
    public Guid OrderId { get; set; }
    public Guid PrintingDetailsId { get; set; }
}