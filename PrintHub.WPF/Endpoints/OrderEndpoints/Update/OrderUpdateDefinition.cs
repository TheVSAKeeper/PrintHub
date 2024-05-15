using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PrintHub.WPF.Definitions.Base;

namespace PrintHub.WPF.Endpoints.OrderEndpoints.Update;

public class OrderUpdateDefinition : AppDefinition
{
    public override void ConfigureServices(IServiceCollection services, HostBuilderContext context)
    {
        services.AddTransient<OrderUpdateFormViewModel>();

        services.AddModalNavigationService<OrderUpdateFormViewModel>();

        services.AddParameterNavigationService<Guid, OrderUpdateFormViewModel>();
        services.AddParameterNavigationService<Guid, NavigateCommand, OrderUpdateFormViewModel>();
    }
}