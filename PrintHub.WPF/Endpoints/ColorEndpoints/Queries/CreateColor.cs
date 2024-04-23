using AutoMapper;
using Calabonga.Results;
using Calabonga.UnitOfWork;
using MediatR;
using Microsoft.Extensions.Logging;
using PrintHub.Domain;
using PrintHub.Domain.Base;
using PrintHub.WPF.Endpoints.ColorEndpoints.ViewModels;

namespace PrintHub.WPF.Endpoints.ColorEndpoints.Queries;

public sealed class CreateColor
{
    public class Handler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<Handler> logger)
        : IRequestHandler<Request, Operation<ColorViewModel, string>>
    {
        public async Task<Operation<ColorViewModel, string>> Handle(Request colorRequest, CancellationToken cancellationToken)
        {
            logger.LogDebug("Creating new Color");

            Color? entity = mapper.Map<ColorCreateViewModel, Color>(colorRequest.Model);

            if (entity == null)
            {
                logger.LogError("Mapper not configured correctly or something went wrong");
                return Operation.Error(AppData.Exceptions.MappingException);
            }

            await unitOfWork.GetRepository<Color>().InsertAsync(entity, cancellationToken);
            await unitOfWork.SaveChangesAsync();

            SaveChangesResult lastResult = unitOfWork.LastSaveChangesResult;

            if (lastResult.IsOk)
            {
                ColorViewModel? mapped = mapper.Map<Color, ColorViewModel>(entity);

                if (mapped is null)
                    return Operation.Error(AppData.Exceptions.MappingException);

                logger.LogInformation("New entity {@Color} successfully created", entity);
                return Operation.Result(mapped);
            }

            string errorMessage = lastResult.Exception?.Message ?? "Something went wrong";
            logger.LogError(errorMessage);
            return Operation.Error(errorMessage);
        }
    }

    public record Request(ColorCreateViewModel Model) : IRequest<Operation<ColorViewModel, string>>;
}