namespace PrintHub.WPF.Shared.Navigation.Modal;

public interface ICallbackNavigationService<out T>
{
    void Navigate(Action<T> parameter);
}