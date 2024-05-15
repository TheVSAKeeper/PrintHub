using System.Windows.Input;
using PrintHub.Domain.Base;
using PrintHub.Infrastructure;
using PrintHub.WPF.Endpoints.AuthenticationEndpoints;
using PrintHub.WPF.Endpoints.AuthenticationEndpoints.Register;
using PrintHub.WPF.Endpoints.AuthenticationEndpoints.Update;
using PrintHub.WPF.Pages.Home;

namespace PrintHub.WPF.Pages.Profile;

public class ProfileViewModel(
    AuthenticationStore authenticationStore,
    NavigationService<HomeViewModel> homeNavigationService,
    ApplicationRoleStore roleStore,
    IMediator mediator,
    IMapper mapper)
    : ViewModelBase
{
    public ApplicationUserUpdateViewModel ApplicationUserUpdateViewModel { get; } = new(authenticationStore, mediator, mapper);
    public RegisterFormViewModel RegisterFormViewModel { get; } = new(authenticationStore, roleStore);

    public bool IsUserAdministrator => authenticationStore.IsInRole(AppData.SystemAdministratorRoleName);

    public ICommand NavigateHomeCommand { get; } = new NavigateCommand(homeNavigationService);
}