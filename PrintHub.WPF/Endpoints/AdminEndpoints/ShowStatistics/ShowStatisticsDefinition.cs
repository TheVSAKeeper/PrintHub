using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PrintHub.WPF.Definitions.Base;
using PrintHub.WPF.Shared.Navigation;

namespace PrintHub.WPF.Endpoints.AdminEndpoints.ShowStatistics;

public class ShowStatisticsDefinition : AppDefinition
{
    public override void ConfigureServices(IServiceCollection services, HostBuilderContext context)
    {
        services.AddTransient<ShowStatisticsFormViewModel>();

        services.AddNavigationService<ShowStatisticsFormViewModel>();
        services.AddModalNavigationService<ShowStatisticsFormViewModel>();
    }
}