namespace PrintHub.WPF.Shared.Navigation.Modal;

public interface ICallbackNavigationService<out T>
{
    void Navigate(Action<T> parameter);
}

public interface IParameterCallbackNavigationService<out T, in TParameter>
{
    void Navigate(Action<T> parameter, TParameter parameter2);
}