using System.Windows.Input;
using PrintHub.WPF.Endpoints.AdminEndpoints;
using PrintHub.WPF.Endpoints.AuthenticationEndpoints;
using PrintHub.WPF.Endpoints.AuthenticationEndpoints.Logout;
using PrintHub.WPF.Pages.Admin;
using PrintHub.WPF.Pages.Client;
using PrintHub.WPF.Pages.Login;
using PrintHub.WPF.Pages.Profile;
using PrintHub.WPF.Shared.Commands;
using PrintHub.WPF.Shared.Navigation;
using PrintHub.WPF.Shared.Navigation.Modal;
using PrintHub.WPF.Shared.ViewModels;

namespace PrintHub.WPF.Pages.Home;

public class HomeViewModel(
    AuthenticationManager authenticationManager,
    NavigationService<ClientViewModel> clientNavigationService,
    NavigationService<AdminViewModel> adminNavigationService,
    NavigationService<ProfileViewModel> profileNavigationService,
    NavigationService<LoginViewModel> loginNavigationService,
    ModalNavigationService<ChangeDbConnectionFormViewModel> changeDbNavigationService)
    : ViewModelBase
{
    public ICommand NavigateClientCommand { get; set; } = new NavigateCommand(clientNavigationService);
    public ICommand NavigateAdminCommand { get; set; } = new NavigateCommand(adminNavigationService);
    public ICommand NavigateProfileCommand { get; } = new NavigateCommand(profileNavigationService);
    public ICommand LogoutCommand { get; } = new LogoutCommand(authenticationManager, loginNavigationService);
    public ICommand ChangeDbCommand { get; } = new NavigateCommand(changeDbNavigationService);

    public string Username => authenticationManager.Username;
}