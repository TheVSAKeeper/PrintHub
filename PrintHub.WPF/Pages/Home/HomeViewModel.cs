using System.Windows.Input;
using PrintHub.WPF.Endpoints.AuthenticationEndpoints;
using PrintHub.WPF.Endpoints.AuthenticationEndpoints.Logout;
using PrintHub.WPF.Endpoints.PrintingDetailsEndpoints;
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
        NavigationService<LoginViewModel> loginNavigationService,
        PrintingDetailsFormViewModel printingDetailsFormViewModel)
    {
        _authenticationManager = authenticationManager;
        PrintingDetailsFormViewModel = printingDetailsFormViewModel;
        PrintingDetailsFormViewModel.SetParameter(Guid.Parse("da6e0f55-a44b-4dff-96c1-5c7ae071962d"));

        NavigateProfileCommand = new NavigateCommand(profileNavigationService);
        LogoutCommand = new LogoutCommand(authenticationManager, loginNavigationService);
    }

    public ICommand NavigateProfileCommand { get; }
    public ICommand LogoutCommand { get; }

    public PrintingDetailsFormViewModel PrintingDetailsFormViewModel { get; }

    public string Username => _authenticationManager.Username;
}