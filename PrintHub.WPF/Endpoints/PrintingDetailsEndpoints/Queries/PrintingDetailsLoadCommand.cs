using Calabonga.Results;
using MediatR;
using PrintHub.WPF.Shared.Commands;

namespace PrintHub.WPF.Endpoints.PrintingDetailsEndpoints.Queries;

public class PrintingDetailsLoadCommand(PrintingDetailsFormViewModel viewModel, IMediator mediator)
    : AsyncCommandBase
{
    protected override async Task ExecuteAsync(object? parameter)
    {
        Operation<PrintingDetailsViewModel, string> result = await mediator.Send(new GetPrintingDetailsById.Request(viewModel.LoadedId));

        if (result.Ok == false)
            return;

        viewModel.Technology = result.Result.Technology;
        viewModel.ColorViewModel = result.Result.ColorViewModel;
        viewModel.MaterialViewModel = result.Result.MaterialViewModel;
    }
}