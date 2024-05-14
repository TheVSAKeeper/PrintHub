using PrintHub.WPF.Shared.Commands;
using PrintHub.WPF.Shared.Navigation;

namespace PrintHub.WPF.Shared.ViewModels;

public interface IParameterViewModel<in T>
{
    public void SetParameter(T parameter);
}

public interface IParameterViewModel<in T1, in T2> where T2 : NavigateCommand
{
    public void SetParameter(T1 parameter, T2 navigateCommand);
}