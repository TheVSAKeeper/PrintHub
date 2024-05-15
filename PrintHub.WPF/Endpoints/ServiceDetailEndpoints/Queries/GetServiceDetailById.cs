using Calabonga.UnitOfWork;
using PrintHub.Domain.Base;
using PrintHub.WPF.Endpoints.ServiceDetailEndpoints.ViewModels;

namespace PrintHub.WPF.Endpoints.ServiceDetailEndpoints.Queries;

public sealed class GetServiceDetailById
{
    public class Handler(IUnitOfWork unitOfWork, IMapper mapper)
        : IRequestHandler<Request, Operation<ServiceDetailViewModel, string>>
    {
        public async Task<Operation<ServiceDetailViewModel, string>> Handle(Request request, CancellationToken cancellationToken)
        {
            Guid id = request.Id;
            IRepository<ServiceDetail> repository = unitOfWork.GetRepository<ServiceDetail>();
            ServiceDetail? entityWithoutIncludes = await repository.GetFirstOrDefaultAsync(predicate: ServiceDetail => ServiceDetail.Id == id);

            if (entityWithoutIncludes == null)
                return Operation.Error($"Entity with identifier {id} not found");

            ServiceDetailViewModel? mapped = mapper.Map<ServiceDetailViewModel>(entityWithoutIncludes);

            if (mapped is not null)
                return Operation.Result(mapped);

            return Operation.Error(AppData.Exceptions.MappingException);
        }
    }

    public record Request(Guid Id) : IRequest<Operation<ServiceDetailViewModel, string>>;
}