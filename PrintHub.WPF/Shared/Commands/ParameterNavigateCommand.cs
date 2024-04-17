using PrintHub.Domain.Exceptions;
using PrintHub.WPF.Shared.Navigation;

namespace PrintHub.WPF.Shared.Commands;

public class ParameterNavigateCommand<T>(IParameterNavigationService<T> navigationService) : CommandBase
{
    protected override void Execute(object? parameter)
    {
        navigationService.Navigate((T)parameter! ?? throw new SurveysArgumentNullException(nameof(parameter)));
    }
}