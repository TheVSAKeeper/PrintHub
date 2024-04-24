using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PrintHub.WPF.Definitions.Base;
using PrintHub.WPF.Endpoints.OrderEndpoints.ViewModels;
using PrintHub.WPF.Shared.Navigation;

namespace PrintHub.WPF.Endpoints.OrderEndpoints;

public class OrderCreateDefinition : AppDefinition
{
    public override void ConfigureServices(IServiceCollection services, HostBuilderContext context)
    {
        services.AddTransient<OrderCreateFormViewModel>();

        services.AddModalNavigationService<OrderCreateFormViewModel>();

        services.AddCallbackNavigationService<OrderViewModel, OrderCreateFormViewModel>();
    }
}