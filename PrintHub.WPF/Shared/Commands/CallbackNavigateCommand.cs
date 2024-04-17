using PrintHub.WPF.Shared.Navigation.Modal;

namespace PrintHub.WPF.Shared.Commands;

public class CallbackNavigateCommand<T>(ICallbackNavigationService<T> navigationService, Action<T> callback) : CommandBase
{
    protected override void Execute(object? parameter)
    {
        navigationService.Navigate(callback);
    }
}