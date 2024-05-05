using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PrintHub.WPF.Definitions.Base;

namespace PrintHub.WPF.Endpoints.AdminEndpoints;

public class DbConnectionServiceDefinition : AppDefinition
{
    public override int OrderIndex => 200;

    public override void ConfigureServices(IServiceCollection services, HostBuilderContext context)
    {
        services.AddScoped<IDbConnectionService, DbConnectionService>();
    }
}