using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PrintHub.WPF.Definitions.Base;
using PrintHub.WPF.Endpoints.AuthenticationEndpoints;

namespace PrintHub.WPF.Definitions.AuthenticationStore;

public class AuthenticationStoreDefinition : AppDefinition
{
    public override void ConfigureServices(IServiceCollection services, HostBuilderContext context)
    {
        services.AddSingleton<AuthenticationManager>();
    }
}