using AutoMapper;
using Calabonga.Results;
using Calabonga.UnitOfWork;
using MediatR;
using PrintHub.Domain;
using PrintHub.Domain.Base;
using PrintHub.WPF.Endpoints.ItemEndpoints.ViewModels;

namespace PrintHub.WPF.Endpoints.ItemEndpoints.Queries;

public sealed class GetItemById
{
    public class Handler(IUnitOfWork unitOfWork, IMapper mapper)
        : IRequestHandler<Request, Operation<ItemViewModel, string>>
    {
        public async Task<Operation<ItemViewModel, string>> Handle(Request request, CancellationToken cancellationToken)
        {
            Guid id = request.Id;
            IRepository<Item> repository = unitOfWork.GetRepository<Item>();
            Item? entityWithoutIncludes = await repository.GetFirstOrDefaultAsync(predicate: Item => Item.Id == id);

            if (entityWithoutIncludes == null)
                return Operation.Error($"Entity with identifier {id} not found");

            ItemViewModel? mapped = mapper.Map<ItemViewModel>(entityWithoutIncludes);

            if (mapped is not null)
                return Operation.Result(mapped);

            return Operation.Error(AppData.Exceptions.MappingException);
        }
    }

    public record Request(Guid Id) : IRequest<Operation<ItemViewModel, string>>;
}