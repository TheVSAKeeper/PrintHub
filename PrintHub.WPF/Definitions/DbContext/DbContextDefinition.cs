using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PrintHub.Infrastructure;
using PrintHub.WPF.Definitions.Base;

namespace PrintHub.WPF.Definitions.DbContext;

public class DbContextDefinition : AppDefinition
{
    public override void ConfigureServices(IServiceCollection services, HostBuilderContext context)
    {
        services.AddDbContext<ApplicationDbContext>(config =>
        {
            config.UseNpgsql(context.Configuration.GetConnectionString(nameof(ApplicationDbContext)));
        });

        services.AddIdentityCore<ApplicationUser>()
            .AddRoles<ApplicationRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            //.AddUserStore<ApplicationUserStore>()
            // .AddRoleStore<ApplicationRoleStore>()
            // .AddUserManager<UserManager<ApplicationUser>>()
            ;

        services.AddScoped<ApplicationUserStore>();
        services.AddScoped<ApplicationRoleStore>();
    }
}