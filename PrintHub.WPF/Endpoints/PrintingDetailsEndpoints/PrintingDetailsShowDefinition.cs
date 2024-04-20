using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PrintHub.WPF.Definitions.Base;
using PrintHub.WPF.Shared.Navigation;

namespace PrintHub.WPF.Endpoints.PrintingDetailsEndpoints;

public class PrintingDetailsShowDefinition : AppDefinition
{
    public override void ConfigureServices(IServiceCollection services, HostBuilderContext context)
    {
        services.AddTransient<PrintingDetailsFormViewModel>();

        services.AddParameterNavigationService<Guid, PrintingDetailsFormViewModel>();
    }
}