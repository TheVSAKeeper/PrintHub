using PrintHub.Domain;
using PrintHub.WPF.Endpoints.ColorEndpoints.ViewModels;
using PrintHub.WPF.Endpoints.MaterialEndpoints.Views;

namespace PrintHub.WPF.Endpoints.PrintingDetailsEndpoints.ViewModels;

public class PrintingDetailsCreateViewModel
{
    public Guid Id { get; set; }

    public PrintingTechnology Technology { get; set; }

    public Guid ColorId { get; set; }
    public ColorViewModel? Color { get; set; }

    public Guid MaterialId { get; set; }
    public MaterialViewModel? Material { get; set; }
}