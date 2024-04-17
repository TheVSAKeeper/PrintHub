using PrintHub.WPF.Shared.Commands;
using PrintHub.WPF.Shared.Navigation;

namespace PrintHub.WPF.Endpoints.AuthenticationEndpoints.Logout;

public class LogoutCommand(AuthenticationManager authenticationManager, INavigationService loginNavigationService)
    : CommandBase
{
    protected override void Execute(object? parameter)
    {
        authenticationManager.SignOut();

        loginNavigationService.Navigate();
    }
}