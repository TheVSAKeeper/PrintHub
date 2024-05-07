using AutoMapper;
using Calabonga.Results;
using Calabonga.UnitOfWork;
using MediatR;
using Microsoft.Extensions.Logging;
using PrintHub.Domain;
using PrintHub.Domain.Base;
using PrintHub.WPF.Endpoints.PrintingDetailsEndpoints.ViewModels;

namespace PrintHub.WPF.Endpoints.PrintingDetailsEndpoints.Queries;

public sealed class CreatePrintingDetails
{
    public class Handler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<Handler> logger)
        : IRequestHandler<Request, Operation<PrintingDetailsViewModel, string>>
    {
        public async Task<Operation<PrintingDetailsViewModel, string>> Handle(Request printingDetailsRequest, CancellationToken cancellationToken)
        {
            logger.LogDebug("Creating new PrintingDetails");

            PrintingDetails? entity = mapper.Map<PrintingDetailsCreateViewModel, PrintingDetails>(printingDetailsRequest.Model);

            if (entity == null)
            {
                logger.LogError("Mapper not configured correctly or something went wrong");
                return Operation.Error(AppData.Exceptions.MappingException);
            }

            await unitOfWork.GetRepository<PrintingDetails>().InsertAsync(entity, cancellationToken);
            await unitOfWork.SaveChangesAsync();

            SaveChangesResult lastResult = unitOfWork.LastSaveChangesResult;

            if (lastResult.IsOk)
            {
                PrintingDetailsViewModel? mapped = mapper.Map<PrintingDetails, PrintingDetailsViewModel>(entity);

                if (mapped is null)
                    return Operation.Error(AppData.Exceptions.MappingException);

                logger.LogInformation("New entity {@PrintingDetails} successfully created", entity);
                return Operation.Result(mapped);
            }

            string errorMessage = lastResult.Exception?.Message ?? "Something went wrong";
            logger.LogError(errorMessage);
            return Operation.Error(errorMessage);
        }
    }

    public record Request(PrintingDetailsCreateViewModel Model) : IRequest<Operation<PrintingDetailsViewModel, string>>;
}