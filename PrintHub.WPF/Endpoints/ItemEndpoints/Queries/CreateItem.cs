using AutoMapper;
using Calabonga.Results;
using Calabonga.UnitOfWork;
using MediatR;
using Microsoft.Extensions.Logging;
using PrintHub.Domain;
using PrintHub.Domain.Base;
using PrintHub.Infrastructure;
using PrintHub.WPF.Endpoints.AuthenticationEndpoints;
using PrintHub.WPF.Endpoints.ItemEndpoints.ViewModels;

namespace PrintHub.WPF.Endpoints.ItemEndpoints.Queries;

public sealed class CreateItem
{
    public class Handler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<Handler> logger, AuthenticationStore authenticationStore)
        : IRequestHandler<Request, Operation<ItemViewModel, string>>
    {
        public async Task<Operation<ItemViewModel, string>> Handle(Request itemRequest, CancellationToken cancellationToken)
        {
            logger.LogDebug("Creating new Item");

            Item? entity = mapper.Map<ItemCreateViewModel, Item>(itemRequest.Model,
                options => options.Items[nameof(ApplicationUser)] = authenticationStore.User?.UserName);

            if (entity == null)
            {
                logger.LogError("Mapper not configured correctly or something went wrong");
                return Operation.Error(AppData.Exceptions.MappingException);
            }

            
            
            await unitOfWork.GetRepository<Item>().InsertAsync(entity, cancellationToken);
            await unitOfWork.SaveChangesAsync();

            SaveChangesResult lastResult = unitOfWork.LastSaveChangesResult;

            if (lastResult.IsOk)
            {
                ItemViewModel? mapped = mapper.Map<Item, ItemViewModel>(entity);

                if (mapped is null)
                    return Operation.Error(AppData.Exceptions.MappingException);

                logger.LogInformation("New entity {@Item} successfully created", entity);
                return Operation.Result(mapped);
            }

            string errorMessage = lastResult.Exception?.Message ?? "Something went wrong";
            logger.LogError(errorMessage);
            return Operation.Error(errorMessage);
        }
    }

    public record Request(ItemCreateViewModel Model) : IRequest<Operation<ItemViewModel, string>>;
}