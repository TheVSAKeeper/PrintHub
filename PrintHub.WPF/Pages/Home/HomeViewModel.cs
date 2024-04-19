using System.Windows.Input;
using PrintHub.WPF.Endpoints.AuthenticationEndpoints;
using PrintHub.WPF.Endpoints.AuthenticationEndpoints.Logout;
using PrintHub.WPF.Pages.Login;
using PrintHub.WPF.Pages.Profile;
using PrintHub.WPF.Shared.Commands;
using PrintHub.WPF.Shared.Navigation;
using PrintHub.WPF.Shared.ViewModels;

namespace PrintHub.WPF.Pages.Home;

public class HomeViewModel : ViewModelBase
{
    private readonly AuthenticationManager _authenticationManager;

    public HomeViewModel(
        AuthenticationManager authenticationManager,
        NavigationService<ProfileViewModel> profileNavigationService,
        NavigationService<LoginViewModel> loginNavigationService
    )
    {
        _authenticationManager = authenticationManager;

        NavigateProfileCommand = new NavigateCommand(profileNavigationService);
        LogoutCommand = new LogoutCommand(authenticationManager, loginNavigationService);
    }

    public ICommand NavigateProfileCommand { get; }
    public ICommand LogoutCommand { get; }

    public string Username => _authenticationManager.Username;
}