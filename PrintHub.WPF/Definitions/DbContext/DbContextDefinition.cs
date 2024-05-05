using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PrintHub.Infrastructure;
using PrintHub.WPF.Definitions.Base;
using PrintHub.WPF.Shared.Services;

namespace PrintHub.WPF.Definitions.DbContext;

public class DbContextDefinition : AppDefinition
{
    public override void ConfigureServices(IServiceCollection services, HostBuilderContext context)
    {
        services.AddDbContext<ApplicationDbContext>(config =>
        {
            config.UseNpgsql(services.BuildServiceProvider().GetRequiredService<IDbConnectionService>().GetConnectionString());
        });

        services.AddIdentityCore<ApplicationUser>()
            .AddRoles<ApplicationRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>();

        services.AddScoped<ApplicationUserStore>();
        services.AddScoped<ApplicationRoleStore>();
    }
}