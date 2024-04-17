using PrintHub.WPF.Shared.Navigation;

namespace PrintHub.WPF.Shared.Commands;

public class NavigateCommand(INavigationService navigationService) : CommandBase
{
    protected override void Execute(object? parameter)
    {
        navigationService.Navigate();
    }
}