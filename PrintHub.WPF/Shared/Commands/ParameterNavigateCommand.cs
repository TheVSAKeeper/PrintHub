using PrintHub.Domain.Exceptions;
using PrintHub.WPF.Shared.Navigation;

namespace PrintHub.WPF.Shared.Commands;

public class ParameterNavigateCommand<T>(IParameterNavigationService<T> navigationService) : CommandBase
{
    protected override void Execute(object? parameter)
    {
        navigationService.Navigate((T)parameter! ?? throw new PrintHubArgumentNullException(nameof(parameter)));
    }
}

public class ParameterBackNavigateCommand<T>(IParameterNavigationService<T, NavigateCommand> navigationService, NavigateCommand command) : CommandBase
{
    protected override void Execute(object? parameter)
    {
        navigationService.Navigate((T)parameter! ?? throw new PrintHubArgumentNullException(nameof(parameter)), command);
    }
}