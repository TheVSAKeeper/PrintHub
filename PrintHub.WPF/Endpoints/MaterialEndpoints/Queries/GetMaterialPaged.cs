using System.Linq.Expressions;
using Calabonga.PagedListCore;
using Calabonga.PredicatesBuilder;
using Calabonga.UnitOfWork;
using PrintHub.Domain.Base;
using PrintHub.WPF.Endpoints.MaterialEndpoints.ViewModels;

namespace PrintHub.WPF.Endpoints.MaterialEndpoints.Queries;

public sealed class GetMaterialPaged
{
    public class Handler(IUnitOfWork unitOfWork, IMapper mapper)
        : IRequestHandler<Request, Operation<IPagedList<MaterialViewModel>, string>>
    {
        public async Task<Operation<IPagedList<MaterialViewModel>, string>> Handle(
            Request request,
            CancellationToken cancellationToken)
        {
            Expression<Func<Material, bool>> predicate = GetPredicate(request.Search);

            IPagedList<Material> pagedList = await unitOfWork.GetRepository<Material>()
                .GetPagedListAsync(predicate,
                    pageIndex: request.PageIndex,
                    pageSize: request.PageSize,
                    cancellationToken: cancellationToken);

            if (pagedList.PageIndex > pagedList.TotalPages)
                pagedList = await unitOfWork.GetRepository<Material>()
                    .GetPagedListAsync(pageIndex: 0,
                        pageSize: request.PageSize, cancellationToken: cancellationToken);

            IPagedList<MaterialViewModel>? mapped = mapper.Map<IPagedList<MaterialViewModel>>(pagedList);

            if (mapped is not null)
                return Operation.Result(mapped);

            return Operation.Error(AppData.Exceptions.MappingException);
        }

        private Expression<Func<Material, bool>> GetPredicate(string? search)
        {
            Expression<Func<Material, bool>>? predicate = PredicateBuilder.True<Material>();

            if (search is null)
                return predicate;

            // predicate = predicate.And(x => x.Message.Contains(search));
            // predicate = predicate.Or(x => x.Logger.Contains(search));
            return predicate;
        }
    }

    public record Request(int PageIndex, int PageSize, string? Search) : IRequest<Operation<IPagedList<MaterialViewModel>, string>>;
}