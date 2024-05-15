using Calabonga.UnitOfWork;
using PrintHub.Domain.Base;
using PrintHub.WPF.Endpoints.MaterialEndpoints.ViewModels;

namespace PrintHub.WPF.Endpoints.MaterialEndpoints.Queries;

public sealed class GetMaterialById
{
    public class Handler(IUnitOfWork unitOfWork, IMapper mapper)
        : IRequestHandler<Request, Operation<MaterialViewModel, string>>
    {
        public async Task<Operation<MaterialViewModel, string>> Handle(Request request, CancellationToken cancellationToken)
        {
            Guid id = request.Id;
            IRepository<Material> repository = unitOfWork.GetRepository<Material>();
            Material? entityWithoutIncludes = await repository.GetFirstOrDefaultAsync(predicate: material => material.Id == id);

            if (entityWithoutIncludes == null)
                return Operation.Error($"Entity with identifier {id} not found");

            MaterialViewModel? mapped = mapper.Map<MaterialViewModel>(entityWithoutIncludes);

            if (mapped is not null)
                return Operation.Result(mapped);

            return Operation.Error(AppData.Exceptions.MappingException);
        }
    }

    public record Request(Guid Id) : IRequest<Operation<MaterialViewModel, string>>;
}