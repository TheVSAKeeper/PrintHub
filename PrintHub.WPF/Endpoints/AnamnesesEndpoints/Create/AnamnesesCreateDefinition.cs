using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PrintHub.Domain;
using PrintHub.WPF.Definitions.Base;
using PrintHub.WPF.Shared.Navigation;

namespace PrintHub.WPF.Endpoints.AnamnesesEndpoints.Create;

public class AnamnesesCreateDefinition : AppDefinition
{
    public override void ConfigureServices(IServiceCollection services, HostBuilderContext context)
    {
        services.AddTransient<AnamnesesCreateFormViewModel>();

        services.AddModalNavigationService<AnamnesesCreateFormViewModel>();

        services.AddCallbackNavigationService<List<Anamnesis>, AnamnesesCreateFormViewModel>();
    }
}