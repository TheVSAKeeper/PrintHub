using System.Linq.Expressions;
using Calabonga.PagedListCore;
using Calabonga.PredicatesBuilder;
using Calabonga.UnitOfWork;
using PrintHub.Domain.Base;
using PrintHub.WPF.Endpoints.ColorEndpoints.ViewModels;

namespace PrintHub.WPF.Endpoints.ColorEndpoints.Queries;

public sealed class GetColorPaged
{
    public class Handler(IUnitOfWork unitOfWork, IMapper mapper)
        : IRequestHandler<Request, Operation<IPagedList<ColorViewModel>, string>>
    {
        public async Task<Operation<IPagedList<ColorViewModel>, string>> Handle(
            Request request,
            CancellationToken cancellationToken)
        {
            Expression<Func<Color, bool>> predicate = GetPredicate(request.Search);

            IPagedList<Color> pagedList = await unitOfWork.GetRepository<Color>()
                .GetPagedListAsync(predicate,
                    pageIndex: request.PageIndex,
                    pageSize: request.PageSize,
                    cancellationToken: cancellationToken);

            if (pagedList.PageIndex > pagedList.TotalPages)
                pagedList = await unitOfWork.GetRepository<Color>()
                    .GetPagedListAsync(pageIndex: 0,
                        pageSize: request.PageSize, cancellationToken: cancellationToken);

            IPagedList<ColorViewModel>? mapped = mapper.Map<IPagedList<ColorViewModel>>(pagedList);

            if (mapped is not null)
                return Operation.Result(mapped);

            return Operation.Error(AppData.Exceptions.MappingException);
        }

        private Expression<Func<Color, bool>> GetPredicate(string? search)
        {
            Expression<Func<Color, bool>>? predicate = PredicateBuilder.True<Color>();

            if (search is null)
                return predicate;

            predicate = predicate.And(x => x.Name.Contains(search));
            predicate = predicate.Or(x => x.ColorCode.Contains(search));
            return predicate;
        }
    }

    public record Request(int PageIndex, int PageSize, string? Search) : IRequest<Operation<IPagedList<ColorViewModel>, string>>;
}