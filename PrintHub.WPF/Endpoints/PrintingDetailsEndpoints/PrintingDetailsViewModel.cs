using PrintHub.WPF.Endpoints.ColorEndpoints;
using PrintHub.WPF.Endpoints.MaterialEndpoints;

namespace PrintHub.WPF.Endpoints.PrintingDetailsEndpoints;

public class PrintingDetailsViewModel
{
    public required string Technology { get; set; }
    public required ColorViewModel ColorViewModel { get; set; }
    public required MaterialViewModel MaterialViewModel { get; set; }
}