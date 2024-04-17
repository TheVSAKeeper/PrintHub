using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PrintHub.Domain;
using PrintHub.WPF.Definitions.Base;
using PrintHub.WPF.Shared.Navigation;

namespace PrintHub.WPF.Endpoints.PatientsEndpoints.Search;

public class PatientSearchDefinition : AppDefinition
{
    public override void ConfigureServices(IServiceCollection services, HostBuilderContext context)
    {
        services.AddTransient<PatientSearchFormViewModel>();

        services.AddModalNavigationService<PatientSearchFormViewModel>();

        services.AddCallbackNavigationService<Patient, PatientSearchFormViewModel>();
    }
}