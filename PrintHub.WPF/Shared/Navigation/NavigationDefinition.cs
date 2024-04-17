using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PrintHub.WPF.Definitions.Base;
using PrintHub.WPF.Shared.Navigation.Modal;

namespace PrintHub.WPF.Shared.Navigation;

public class NavigationDefinition : AppDefinition
{
    public override void ConfigureServices(IServiceCollection services, HostBuilderContext context)
    {
        services.AddSingleton<NavigationMediator>();
        services.AddSingleton<ModalNavigationMediator>();
        services.AddSingleton<CloseModalNavigationService>();
    }
}