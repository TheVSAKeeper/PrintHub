using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PrintHub.WPF.Definitions.Base;
using PrintHub.WPF.Shared.Navigation;

namespace PrintHub.WPF.Endpoints.SurveysEndpoints.ShowAll;

public class SurveyShowAllSearchDefinition : AppDefinition
{
    public override void ConfigureServices(IServiceCollection services, HostBuilderContext context)
    {
        services.AddTransient<SurveyShowAllFormViewModel>();

        services.AddNavigationService<SurveyShowAllFormViewModel>();
        services.AddModalNavigationService<SurveyShowAllFormViewModel>();
    }
}