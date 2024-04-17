using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PrintHub.WPF.Definitions.Base;
using PrintHub.WPF.Shared.Navigation;

namespace PrintHub.WPF.Endpoints.SurveysEndpoints.Edit;

public class SurveyEditDefinition : AppDefinition
{
    public override void ConfigureServices(IServiceCollection services, HostBuilderContext context)
    {
        services.AddTransient<SurveyEditFormViewModel>();

        services.AddParameterNavigationService<Guid, SurveyEditFormViewModel>();
    }
}