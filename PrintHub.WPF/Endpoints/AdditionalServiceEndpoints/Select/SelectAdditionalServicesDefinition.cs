using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PrintHub.WPF.Definitions.Base;

namespace PrintHub.WPF.Endpoints.AdditionalServiceEndpoints.Select;

public class SelectAdditionalServicesDefinition : AppDefinition
{
    public override void ConfigureServices(IServiceCollection services, HostBuilderContext context)
    {
        services.AddTransient<SelectAdditionalServicesFormViewModel>();

        services.AddNavigationService<SelectAdditionalServicesFormViewModel>();
        services.AddModalNavigationService<SelectAdditionalServicesFormViewModel>();
    }
}