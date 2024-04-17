namespace PrintHub.WPF.Shared.Navigation;

public interface IParameterNavigationService<in TParameter>
{
    void Navigate(TParameter parameter);
}