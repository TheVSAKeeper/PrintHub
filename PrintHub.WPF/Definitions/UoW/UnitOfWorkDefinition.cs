using Calabonga.UnitOfWork;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PrintHub.Infrastructure;
using PrintHub.WPF.Definitions.Base;

namespace PrintHub.WPF.Definitions.UoW;

public class UnitOfWorkDefinition : AppDefinition
{
    public override void ConfigureServices(IServiceCollection services, HostBuilderContext context)
        => services.AddUnitOfWork<ApplicationDbContext>();
}