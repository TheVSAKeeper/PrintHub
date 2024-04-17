using PrintHub.WPF.Shared.ViewModels;

namespace PrintHub.WPF.Shared.Navigation.Modal;

public class CallbackModalNavigationService<TParameter, TViewModel>(ModalNavigationMediator navigationMediator, CreateViewModel<Action<TParameter>, TViewModel> createViewModel)
    : ICallbackNavigationService<TParameter> where TViewModel : ViewModelBase, ICallbackViewModel<TParameter>
{
    public void Navigate(Action<TParameter> parameter)
    {
        navigationMediator.CurrentViewModel = createViewModel(parameter);
    }
}