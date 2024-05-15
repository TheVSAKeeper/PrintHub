using System.Linq.Expressions;
using Calabonga.PagedListCore;
using Calabonga.PredicatesBuilder;
using Calabonga.UnitOfWork;
using PrintHub.Domain.Base;
using PrintHub.WPF.Endpoints.AdditionalServiceEndpoints.ViewModels;

namespace PrintHub.WPF.Endpoints.AdditionalServiceEndpoints.Queries;

public sealed class GetAdditionalServicePaged
{
    public class Handler(IUnitOfWork unitOfWork, IMapper mapper)
        : IRequestHandler<Request, Operation<IPagedList<AdditionalServiceViewModel>, string>>
    {
        public async Task<Operation<IPagedList<AdditionalServiceViewModel>, string>> Handle(
            Request request,
            CancellationToken cancellationToken)
        {
            Expression<Func<AdditionalService, bool>> predicate = GetPredicate(request.Search);

            IPagedList<AdditionalService> pagedList = await unitOfWork.GetRepository<AdditionalService>()
                .GetPagedListAsync(predicate,
                    pageIndex: request.PageIndex,
                    pageSize: request.PageSize,
                    cancellationToken: cancellationToken);

            if (pagedList.PageIndex > pagedList.TotalPages)
                pagedList = await unitOfWork.GetRepository<AdditionalService>()
                    .GetPagedListAsync(pageIndex: 0,
                        pageSize: request.PageSize, cancellationToken: cancellationToken);

            IPagedList<AdditionalServiceViewModel>? mapped = mapper.Map<IPagedList<AdditionalServiceViewModel>>(pagedList);

            if (mapped is not null)
                return Operation.Result(mapped);

            return Operation.Error(AppData.Exceptions.MappingException);
        }

        private Expression<Func<AdditionalService, bool>> GetPredicate(string? search)
        {
            Expression<Func<AdditionalService, bool>>? predicate = PredicateBuilder.True<AdditionalService>();

            if (search is null)
                return predicate;

            // predicate = predicate.And(x => x.Message.Contains(search));
            // predicate = predicate.Or(x => x.Logger.Contains(search));
            return predicate;
        }
    }

    public record Request(int PageIndex, int PageSize, string? Search) : IRequest<Operation<IPagedList<AdditionalServiceViewModel>, string>>;
}