using System.Windows.Controls;
using Microsoft.AspNetCore.Identity;
using PrintHub.WPF.Shared.Commands;
using PrintHub.WPF.Shared.MaterialMessageBox;

namespace PrintHub.WPF.Endpoints.AuthenticationEndpoints.Register;

public class RegisterCommand(
    RegisterFormViewModel registerViewModel,
    AuthenticationManager authenticationManager)
    : AsyncCommandBase
{
    protected override async Task ExecuteAsync(object? parameter)
    {
        try
        {
            if (parameter is not PasswordBox passwordBox)
                return;

            IdentityResult result = await authenticationManager.CreateUserAsync(registerViewModel.Username!, passwordBox.Password, registerViewModel.Role!);

            if (result.Succeeded == false)
            {
                string errors = string.Join(Environment.NewLine, result.Errors.Select(error => error.Description));
                MaterialMessageBox.ShowError($"Ошибка регистрации. {Environment.NewLine}{errors}", "Ошибка");
                return;
            }

            MaterialMessageBox.Show("Успешно зарегистрирован!", "Успех");
        }
        catch (Exception)
        {
            MaterialMessageBox.ShowError("Регистрация не удалась. Пожалуйста, проверьте вашу информацию или повторите попытку позже.", "Ошибка");
        }
    }

    protected override bool CanExecuteAsync(object? parameter) => registerViewModel is
    {
        Username: not null,
        Role: not null
    };
}