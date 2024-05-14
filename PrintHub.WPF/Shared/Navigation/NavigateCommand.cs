using PrintHub.WPF.Shared.Commands;

namespace PrintHub.WPF.Shared.Navigation;

public class NavigateCommand(INavigationService navigationService) : CommandBase
{
    protected override void Execute(object? parameter)
    {
        navigationService.Navigate();
    }
}