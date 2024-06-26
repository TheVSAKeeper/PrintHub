using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PrintHub.WPF.Definitions.Base;
using PrintHub.WPF.Endpoints.ItemEndpoints.ViewModels;

namespace PrintHub.WPF.Endpoints.ItemEndpoints.Create;

public class ItemCreateDefinition : AppDefinition
{
    public override void ConfigureServices(IServiceCollection services, HostBuilderContext context)
    {
        services.AddTransient<ItemCreateFormViewModel>();

        services.AddModalNavigationService<ItemCreateFormViewModel>();

        services.AddParameterCallbackNavigationService<ItemViewModel, Guid, ItemCreateFormViewModel>();
    }
}