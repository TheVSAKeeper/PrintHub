using System.Windows.Controls;
using Microsoft.AspNetCore.Identity;
using PrintHub.WPF.Shared.MaterialMessageBox;

namespace PrintHub.WPF.Endpoints.AuthenticationEndpoints.Login;

public class LoginCommand(
    LoginFormViewModel loginViewModel,
    AuthenticationStore authenticationStore,
    INavigationService homeNavigationService)
    : AsyncCommandBase
{
    protected override async Task ExecuteAsync(object? parameter)
    {
        try
        {
            if (parameter is not PasswordBox passwordBox)
                return;

            SignInResult result = await authenticationStore.SignInAsync(loginViewModel.Username!, passwordBox.Password);

            if (result.Succeeded == false)
            {
                MaterialMessageBox.ShowError("Неверный логин или пароль.", "Ошибка");
                return;
            }

            homeNavigationService.Navigate();
        }
        catch (Exception)
        {
            MaterialMessageBox.ShowError("Ошибка входа. Пожалуйста, проверьте вашу информацию или повторите попытку позже.", "Ошибка");
        }
    }

    protected override bool CanExecuteAsync(object? parameter) => loginViewModel.Username != null;
}