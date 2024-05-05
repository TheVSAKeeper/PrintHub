using PrintHub.WPF.Shared.Commands;
using PrintHub.WPF.Shared.Navigation;

namespace PrintHub.WPF.Endpoints.AuthenticationEndpoints.Logout;

public class LogoutCommand(AuthenticationStore authenticationStore, INavigationService loginNavigationService)
    : CommandBase
{
    protected override void Execute(object? parameter)
    {
        authenticationStore.SignOut();

        loginNavigationService.Navigate();
    }
}