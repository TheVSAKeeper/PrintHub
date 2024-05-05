using System.Windows.Input;
using PrintHub.WPF.Pages.Home;
using PrintHub.WPF.Pages.Login;
using PrintHub.WPF.Shared.Commands;
using PrintHub.WPF.Shared.Navigation.Modal;
using PrintHub.WPF.Shared.ViewModels;

namespace PrintHub.WPF.Shared.Navigation.Bar;

public class NavigationBarViewModel(NavigationService<HomeViewModel> homeNavigationService, CloseModalNavigationService closeNavigationService) : ViewModelBase
{
    private ICommand? _backCommand;

    public ICommand BackCommand => _backCommand ??= new LambdaCommand(() =>
    {
        if (MainViewModel?.CurrentModalViewModel is null)
        {
            NavigateHomeCommand.Execute(null);
            return;
        }

        CloseModalCommand.Execute(null);
    });

    private ICommand CloseModalCommand { get; } = new NavigateCommand(closeNavigationService);
    public ICommand NavigateHomeCommand { get; } = new NavigateCommand(homeNavigationService);
    public MainViewModel? MainViewModel { get; set; }

    public bool IsEnabledHomeButton => MainViewModel?.CurrentViewModel is not LoginViewModel;
}