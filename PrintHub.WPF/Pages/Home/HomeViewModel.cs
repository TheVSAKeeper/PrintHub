using System.Windows.Input;
using MediatR;
using PrintHub.WPF.Endpoints.AuthenticationEndpoints;
using PrintHub.WPF.Endpoints.AuthenticationEndpoints.Logout;
using PrintHub.WPF.Endpoints.OrderEndpoints;
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
        IMediator mediator,
        AuthenticationManager authenticationManager,
        NavigationService<ProfileViewModel> profileNavigationService,
        NavigationService<LoginViewModel> loginNavigationService)
    {
        _authenticationManager = authenticationManager;
        NavigateProfileCommand = new NavigateCommand(profileNavigationService);
        OrderCreateFormViewModel = new OrderCreateFormViewModel(mediator, authenticationManager);
        LogoutCommand = new LogoutCommand(authenticationManager, loginNavigationService);
    }

    public ICommand NavigateProfileCommand { get; }
    public ICommand LogoutCommand { get; }
    public OrderCreateFormViewModel OrderCreateFormViewModel { get; }
    public string Username => _authenticationManager.Username;
}