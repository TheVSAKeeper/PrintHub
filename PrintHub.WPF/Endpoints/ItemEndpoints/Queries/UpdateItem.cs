using AutoMapper;
using Calabonga.Results;
using Calabonga.UnitOfWork;
using MediatR;
using PrintHub.Domain;
using PrintHub.Domain.Base;
using PrintHub.WPF.Endpoints.ItemEndpoints.ViewModels;

namespace PrintHub.WPF.Endpoints.ItemEndpoints.Queries;

public sealed class UpdateItem
{
    public class Handler(IUnitOfWork unitOfWork, IMapper mapper)
        : IRequestHandler<Request, Operation<ItemViewModel, string>>
    {
        public async Task<Operation<ItemViewModel, string>> Handle(Request ItemRequest, CancellationToken cancellationToken)
        {
            IRepository<Item> repository = unitOfWork.GetRepository<Item>();

            Item? entity = await repository.GetFirstOrDefaultAsync(predicate: Item => Item.Id == ItemRequest.Id, disableTracking: false);

            if (entity == null)
                return Operation.Error(AppData.Exceptions.NotFoundException);

            mapper.Map(ItemRequest.Model, entity);

            repository.Update(entity);
            await unitOfWork.SaveChangesAsync();

            SaveChangesResult lastResult = unitOfWork.LastSaveChangesResult;

            if (lastResult.IsOk)
            {
                ItemViewModel? mapped = mapper.Map<Item, ItemViewModel>(entity);

                if (mapped is not null)
                    return Operation.Result(mapped);

                return Operation.Error(AppData.Exceptions.MappingException);
            }

            string errorMessage = lastResult.Exception?.Message ?? "Something went wrong";
            return Operation.Error(errorMessage);
        }
    }

    public record Request(Guid Id, ItemUpdateViewModel Model) : IRequest<Operation<ItemViewModel, string>>;
}