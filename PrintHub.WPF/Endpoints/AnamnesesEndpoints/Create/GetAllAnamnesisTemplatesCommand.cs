﻿using System.Collections.ObjectModel;
using Calabonga.OperationResults;
using MediatR;
using PrintHub.WPF.Shared.Commands;

namespace PrintHub.WPF.Endpoints.AnamnesesEndpoints.Create;

public class GetAllAnamnesisTemplatesCommand(AnamnesesCreateFormViewModel viewModel, IMediator mediator)
    : AsyncCommandBase
{
    protected override async Task ExecuteAsync(object? parameter)
    {
        OperationResult<List<AnamnesisTemplateDto>> result = await mediator.Send(new GetAllAnamnesisTemplatesRequest());

        if (result.Ok)
            viewModel.AnamnesisTemplates = new ObservableCollection<AnamnesisTemplateDto>(result.Result!);
    }
}