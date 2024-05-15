using Calabonga.UnitOfWork;
using PrintHub.Domain.Base;
using PrintHub.WPF.Endpoints.ItemEndpoints.ViewModels;

namespace PrintHub.WPF.Endpoints.ItemEndpoints.Queries;

public sealed class DeleteItem
{
    public class Handler(IUnitOfWork unitOfWork, IMapper mapper)
        : IRequestHandler<Request, Operation<ItemViewModel, string>>
    {
        public async Task<Operation<ItemViewModel, string>> Handle(Request request, CancellationToken cancellationToken)
        {
            IRepository<Item> repository = unitOfWork.GetRepository<Item>();
            Item? entity = await repository.FindAsync([request.Id], cancellationToken);

            if (entity == null)
                return Operation.Error("Entity not found");

            repository.Delete(entity);
            await unitOfWork.SaveChangesAsync();

            if (unitOfWork.LastSaveChangesResult.IsOk == false)
                return Operation.Error(unitOfWork.LastSaveChangesResult.Exception?.Message ?? AppData.Exceptions.SomethingWrong);

            ItemViewModel? mapped = mapper.Map<ItemViewModel>(entity);

            if (mapped is not null)
                return Operation.Result(mapped);

            return Operation.Error(AppData.Exceptions.MappingException);
        }
    }

    public record Request(Guid Id) : IRequest<Operation<ItemViewModel, string>>;
}