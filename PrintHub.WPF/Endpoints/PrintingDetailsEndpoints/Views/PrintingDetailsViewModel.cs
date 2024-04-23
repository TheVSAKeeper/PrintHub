using PrintHub.WPF.Endpoints.ColorEndpoints.ViewModels;
using PrintHub.WPF.Endpoints.MaterialEndpoints.Views;

namespace PrintHub.WPF.Endpoints.PrintingDetailsEndpoints.Views;

public class PrintingDetailsViewModel
{
    public required string Technology { get; set; }
    public required ColorViewModel ColorViewModel { get; set; }
    public required MaterialViewModel MaterialViewModel { get; set; }
}