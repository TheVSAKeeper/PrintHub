namespace PrintHub.WPF.Shared.ViewModels;

public interface ICallbackViewModel<out T>
{
    public void SetCallback(Action<T> callback);
}

public interface ICallbackViewModel<out T, in TParameter>
{
    public void SetCallback(Action<T> callback, TParameter parameter);
}