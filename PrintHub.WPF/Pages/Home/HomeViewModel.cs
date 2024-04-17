using System.Windows.Input;
using PrintHub.WPF.Endpoints.AuthenticationEndpoints;
using PrintHub.WPF.Endpoints.AuthenticationEndpoints.Logout;
using PrintHub.WPF.Endpoints.SurveysEndpoints.ShowAll;
using PrintHub.WPF.Pages.Login;
using PrintHub.WPF.Pages.Profile;
using PrintHub.WPF.Pages.Survey;
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
        NavigationService<SurveyViewModel> surveyNavigationService,
        SurveyShowAllFormViewModel showAllFormViewModel)
    {
        _authenticationManager = authenticationManager;

        NavigateProfileCommand = new NavigateCommand(profileNavigationService);
        OpenSurveysCommand = new NavigateCommand(surveyNavigationService);
        LogoutCommand = new LogoutCommand(authenticationManager, loginNavigationService);
        SurveyShowAllFormViewModel = showAllFormViewModel;
    }

    public SurveyShowAllFormViewModel SurveyShowAllFormViewModel { get; }
    public ICommand NavigateProfileCommand { get; }
    public ICommand OpenSurveysCommand { get; }
    public ICommand LogoutCommand { get; }

    public string Username => _authenticationManager.User?.DisplayName ?? "Unknown";
}