using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PrintHub.WPF.Definitions.Base;
using PrintHub.WPF.Shared.Navigation;

namespace PrintHub.WPF.Endpoints.AdminEndpoints;

public class ChangeDbConnectionDefinition : AppDefinition
{
    public override void ConfigureServices(IServiceCollection services, HostBuilderContext context)
    {
        services.AddTransient<ChangeDbConnectionFormViewModel>();

        services.AddNavigationService<ChangeDbConnectionFormViewModel>();
        services.AddModalNavigationService<ChangeDbConnectionFormViewModel>();
    }
}