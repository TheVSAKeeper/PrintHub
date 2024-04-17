using System.Windows.Input;
using AutoMapper;
using MediatR;
using PrintHub.Domain.Base;
using PrintHub.Infrastructure;
using PrintHub.WPF.Endpoints.AuthenticationEndpoints;
using PrintHub.WPF.Endpoints.AuthenticationEndpoints.Register;
using PrintHub.WPF.Endpoints.AuthenticationEndpoints.Update;
using PrintHub.WPF.Pages.Home;
using PrintHub.WPF.Shared.Commands;
using PrintHub.WPF.Shared.Navigation;
using PrintHub.WPF.Shared.ViewModels;

namespace PrintHub.WPF.Pages.Profile;

public class ProfileViewModel(
    AuthenticationManager authenticationManager,
    NavigationService<HomeViewModel> homeNavigationService,
    ApplicationRoleStore roleStore,
    IMediator mediator,
    IMapper mapper)
    : ViewModelBase
{
    public ApplicationUserUpdateViewModel ApplicationUserUpdateViewModel { get; } = new(authenticationManager, mediator, mapper);
    public RegisterFormViewModel RegisterFormViewModel { get; } = new(authenticationManager, roleStore);

    public bool IsUserAdministrator => authenticationManager.IsInRole(AppData.SystemAdministratorRoleName);

    public ICommand NavigateHomeCommand { get; } = new NavigateCommand(homeNavigationService);
}