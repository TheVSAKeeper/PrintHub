using PrintHub.WPF.Shared.Commands;
using PrintHub.WPF.Shared.ViewModels;

namespace PrintHub.WPF.Shared.Navigation;

public class ParameterNavigationService<TParameter, TViewModel>(INavigationMediator navigationMediator, Func<TParameter, TViewModel> createViewModel)
    : IParameterNavigationService<TParameter> where TViewModel : ViewModelBase
{
    public void Navigate(TParameter parameter)
    {
        navigationMediator.CurrentViewModel = createViewModel(parameter);
    }
}

public class ParameterNavigationService<TParameter, TCommand, TViewModel>(INavigationMediator navigationMediator, Func<TParameter, TCommand, TViewModel> createViewModel)
    : IParameterNavigationService<TParameter, TCommand> where TViewModel : ViewModelBase where TCommand : NavigateCommand
{
    public void Navigate(TParameter parameter, TCommand navigateCommand)
    {
        navigationMediator.CurrentViewModel = createViewModel(parameter, navigateCommand);
    }
}