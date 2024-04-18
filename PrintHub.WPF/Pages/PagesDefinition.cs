using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PrintHub.WPF.Definitions.Base;
using PrintHub.WPF.Pages.Home;
using PrintHub.WPF.Pages.Login;
using PrintHub.WPF.Pages.Profile;

namespace PrintHub.WPF.Pages;

public class PagesDefinition : AppDefinition
{
    public override void ConfigureServices(IServiceCollection services, HostBuilderContext context)
    {
        services.AddPage<HomeViewModel>();
        services.AddPage<LoginViewModel>();
        services.AddPage<ProfileViewModel>();
    }
}