using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PrintHub.WPF.Definitions.Base;
using PrintHub.WPF.Endpoints.PrintingDetailsEndpoints.ViewModels;
using PrintHub.WPF.Shared.Navigation;

namespace PrintHub.WPF.Endpoints.PrintingDetailsEndpoints.Create;

public class PrintingDetailsCreateDefinition : AppDefinition
{
    public override void ConfigureServices(IServiceCollection services, HostBuilderContext context)
    {
        services.AddTransient<PrintingDetailsCreateFormViewModel>();

        services.AddModalNavigationService<PrintingDetailsCreateFormViewModel>();

        services.AddCallbackModalNavigationService<PrintingDetailsViewModel, PrintingDetailsCreateFormViewModel>();
    }
}