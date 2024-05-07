using PrintHub.WPF.Shared.Commands;

namespace PrintHub.WPF.Shared.Navigation;

public interface IParameterNavigationService<in TParameter>
{
    void Navigate(TParameter parameter);
}

public interface IParameterNavigationService<in TParameter, in T2> where T2 : NavigateCommand
{
    void Navigate(TParameter parameter, T2 navigateCommand);
}