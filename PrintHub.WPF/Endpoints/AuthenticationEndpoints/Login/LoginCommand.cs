using System.Windows;
using System.Windows.Controls;
using Microsoft.AspNetCore.Identity;
using PrintHub.WPF.Shared.Commands;
using PrintHub.WPF.Shared.Navigation;

namespace PrintHub.WPF.Endpoints.AuthenticationEndpoints.Login;

public class LoginCommand(
    LoginFormViewModel loginViewModel,
    AuthenticationManager authenticationManager,
    INavigationService homeNavigationService)
    : AsyncCommandBase
{
    protected override async Task ExecuteAsync(object? parameter)
    {
        try
        {
            if (parameter is not PasswordBox passwordBox)
                return;

            SignInResult result = await authenticationManager.SignInAsync(loginViewModel.Username!, passwordBox.Password);

            if (result.Succeeded == false)
            {
                MessageBox.Show("Неверный логин или пароль.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            homeNavigationService.Navigate();
        }
        catch (Exception)
        {
            MessageBox.Show("Ошибка входа. Пожалуйста, проверьте вашу информацию или повторите попытку позже.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    protected override bool CanExecuteAsync(object? parameter) => loginViewModel.Username != null;
}