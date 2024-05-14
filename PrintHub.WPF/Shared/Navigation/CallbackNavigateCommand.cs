using PrintHub.Domain.Exceptions;
using PrintHub.WPF.Shared.Commands;
using PrintHub.WPF.Shared.Navigation.Modal;

namespace PrintHub.WPF.Shared.Navigation;

public class CallbackNavigateCommand<T>(ICallbackNavigationService<T> navigationService, Action<T> callback) : CommandBase
{
    protected override void Execute(object? parameter)
    {
        navigationService.Navigate(callback);
    }
}

public class ParameterCallbackNavigateCommand<TParameter, T>(IParameterCallbackNavigationService<T, TParameter> navigationService, Action<T> callback) : CommandBase
{
    protected override void Execute(object? parameter)
    {
        navigationService.Navigate(callback, (TParameter)parameter! ?? throw new PrintHubArgumentNullException(nameof(parameter)));
    }
}