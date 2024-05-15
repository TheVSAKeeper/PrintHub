using PrintHub.WPF.Endpoints.ColorEndpoints.ViewModels;
using MaterialViewModel = PrintHub.WPF.Endpoints.MaterialEndpoints.ViewModels.MaterialViewModel;

namespace PrintHub.WPF.Endpoints.PrintingDetailsEndpoints.ViewModels;

public class PrintingDetailsViewModel
{
    public Guid Id { get; set; }
    public PrintingTechnology Technology { get; set; }
    public ColorViewModel ColorViewModel { get; set; }
    public MaterialViewModel MaterialViewModel { get; set; }
    public override string ToString() => $"{nameof(Id)}: {Id}, {nameof(Technology)}: {Technology}, {nameof(ColorViewModel)}: {ColorViewModel}, {nameof(MaterialViewModel)}: {MaterialViewModel}";
}