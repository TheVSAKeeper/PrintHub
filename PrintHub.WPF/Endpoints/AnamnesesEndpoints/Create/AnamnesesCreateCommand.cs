using Calabonga.OperationResults;
using MediatR;
using PrintHub.Domain;
using PrintHub.WPF.Shared.Commands;

namespace PrintHub.WPF.Endpoints.AnamnesesEndpoints.Create;

public class AnamnesesCreateCommand(AnamnesesCreateFormViewModel viewModel, IMediator mediator)
    : AsyncCommandBase
{
    protected override async Task ExecuteAsync(object? parameter)
    {
        OperationResult<List<Anamnesis>> result =
            await mediator.Send(new AnamnesesCreateRequest(viewModel.AnamnesisTemplates!.ToList()));

        if (result.Ok == false)
            return;

        // MessageBox.Show($"Создано {result.Result!.Count} анамнезов", "Успех", MessageBoxButton.OK, MessageBoxImage.None);
        viewModel.CreatedAnamneses = result.Result;

        foreach (AnamnesisTemplateDto template in viewModel.AnamnesisTemplates!)
            template.IsSelected = false;

        viewModel.CancelCommand.Execute(null);
    }

    protected override bool CanExecuteAsync(object? parameter) => viewModel.AnamnesisTemplates != null
                                                                  && viewModel.AnamnesisTemplates.Any(template => template.IsSelected);
}