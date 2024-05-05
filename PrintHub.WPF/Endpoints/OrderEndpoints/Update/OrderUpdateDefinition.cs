using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PrintHub.WPF.Definitions.Base;
using PrintHub.WPF.Shared.Navigation;

namespace PrintHub.WPF.Endpoints.OrderEndpoints.Update;

public class OrderUpdateDefinition : AppDefinition
{
    public override void ConfigureServices(IServiceCollection services, HostBuilderContext context)
    {
        services.AddTransient<OrderUpdateFormViewModel>();

        services.AddModalNavigationService<OrderUpdateFormViewModel>();
    }
}