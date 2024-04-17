using System.Collections.ObjectModel;
using Calabonga.OperationResults;
using MediatR;
using PrintHub.Domain;
using PrintHub.WPF.Shared.Commands;

namespace PrintHub.WPF.Endpoints.PatientsEndpoints.Search;

public class GetAllPatientsCommand(PatientSearchFormViewModel viewModel, IMediator mediator)
    : AsyncCommandBase
{
    protected override async Task ExecuteAsync(object? parameter)
    {
        OperationResult<List<Patient>> result = await mediator.Send(new GetAllPatientsRequest());

        if (result.Ok)
            viewModel.Patients = new ObservableCollection<Patient>(result.Result!);
    }
}