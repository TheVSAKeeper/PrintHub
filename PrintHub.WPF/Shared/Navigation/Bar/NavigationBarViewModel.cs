using System.Windows.Input;
using PrintHub.WPF.Pages.Home;
using PrintHub.WPF.Pages.Login;
using PrintHub.WPF.Shared.Commands;
using PrintHub.WPF.Shared.ViewModels;

namespace PrintHub.WPF.Shared.Navigation.Bar;

public class NavigationBarViewModel(NavigationService<HomeViewModel> homeNavigationService) : ViewModelBase
{
    public MainViewModel? MainViewModel { get; set; }

    public ICommand NavigateHomeCommand { get; } = new NavigateCommand(homeNavigationService);

    public bool IsEnabledHomeButton => MainViewModel?.CurrentViewModel is not LoginViewModel;
}