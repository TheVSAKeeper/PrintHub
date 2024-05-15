using Calabonga.UnitOfWork;
using PrintHub.Domain.Base;
using PrintHub.WPF.Endpoints.AdditionalServiceEndpoints.ViewModels;

namespace PrintHub.WPF.Endpoints.AdditionalServiceEndpoints.Queries;

public sealed class GetAdditionalServiceById
{
    public class Handler(IUnitOfWork unitOfWork, IMapper mapper)
        : IRequestHandler<Request, Operation<AdditionalServiceViewModel, string>>
    {
        public async Task<Operation<AdditionalServiceViewModel, string>> Handle(Request request, CancellationToken cancellationToken)
        {
            Guid id = request.Id;
            IRepository<AdditionalService> repository = unitOfWork.GetRepository<AdditionalService>();
            AdditionalService? entityWithoutIncludes = await repository.GetFirstOrDefaultAsync(predicate: AdditionalService => AdditionalService.Id == id);

            if (entityWithoutIncludes == null)
                return Operation.Error($"Entity with identifier {id} not found");

            AdditionalServiceViewModel? mapped = mapper.Map<AdditionalServiceViewModel>(entityWithoutIncludes);

            if (mapped is not null)
                return Operation.Result(mapped);

            return Operation.Error(AppData.Exceptions.MappingException);
        }
    }

    public record Request(Guid Id) : IRequest<Operation<AdditionalServiceViewModel, string>>;
}