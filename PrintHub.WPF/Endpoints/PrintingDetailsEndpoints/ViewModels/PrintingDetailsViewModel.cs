using PrintHub.WPF.Endpoints.ColorEndpoints.ViewModels;
using PrintHub.WPF.Endpoints.MaterialEndpoints.Views;

namespace PrintHub.WPF.Endpoints.PrintingDetailsEndpoints.ViewModels;

public class PrintingDetailsViewModel
{
    public Guid Id { get; set; }

    public string Technology { get; set; }
    public ColorViewModel ColorViewModel { get; set; }
    public MaterialViewModel MaterialViewModel { get; set; }
}