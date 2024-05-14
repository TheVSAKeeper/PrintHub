using PrintHub.WPF.Shared.ViewModels;

namespace PrintHub.WPF.Shared.Navigation.Modal;

public class CallbackModalNavigationService<TParameter, TViewModel>(INavigationMediator navigationMediator, CreateViewModel<Action<TParameter>, TViewModel> createViewModel)
    : ICallbackNavigationService<TParameter> where TViewModel : ViewModelBase, ICallbackViewModel<TParameter>
{
    public void Navigate(Action<TParameter> parameter)
    {
        navigationMediator.CurrentViewModel = createViewModel(parameter);
    }
}

public class ParameterCallbackModalNavigationService<TParameter, T, TViewModel>(INavigationMediator navigationMediator, CreateViewModel<Action<TParameter>, T, TViewModel> createViewModel)
    : IParameterCallbackNavigationService<TParameter, T> where TViewModel : ViewModelBase, ICallbackViewModel<TParameter, T>
{
    public void Navigate(Action<TParameter> parameter, T parameter2)
    {
        navigationMediator.CurrentViewModel = createViewModel(parameter, parameter2);
    }
}