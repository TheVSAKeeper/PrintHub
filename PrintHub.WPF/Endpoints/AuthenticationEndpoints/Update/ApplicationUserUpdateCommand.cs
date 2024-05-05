using Calabonga.OperationResults;
using MediatR;
using PrintHub.WPF.Shared.Commands;
using PrintHub.WPF.Shared.MaterialMessageBox;

namespace PrintHub.WPF.Endpoints.AuthenticationEndpoints.Update;

public class ApplicationUserUpdateCommand(ApplicationUserUpdateViewModel viewModel, IMediator mediator, AuthenticationStore authenticationStore)
    : AsyncCommandBase
{
    protected override async Task ExecuteAsync(object? parameter)
    {
        viewModel.Validate();

        if (viewModel.HasErrors)
            return;

        OperationResult<Guid> result = await mediator.Send(new ApplicationUserUpdateRequest(viewModel.ApplicationUser));
        await authenticationStore.UpdateUserAsync(result.Result);

        if (result.Ok)
            MaterialMessageBox.Show("Пользователь обновлен.", "Успех");
        else
            MaterialMessageBox.ShowError("Ошибка обновления пользователя.", "Ошибка");
    }
}