using System.Linq.Expressions;
using Calabonga.PagedListCore;
using Calabonga.PredicatesBuilder;
using Calabonga.UnitOfWork;
using PrintHub.Domain.Base;
using PrintHub.WPF.Endpoints.PrintingDetailsEndpoints.ViewModels;

namespace PrintHub.WPF.Endpoints.PrintingDetailsEndpoints.Queries;

public sealed class GetPrintingDetailsPaged
{
    public class Handler(IUnitOfWork unitOfWork, IMapper mapper)
        : IRequestHandler<Request, Operation<IPagedList<PrintingDetailsViewModel>, string>>
    {
        public async Task<Operation<IPagedList<PrintingDetailsViewModel>, string>> Handle(
            Request request,
            CancellationToken cancellationToken)
        {
            Expression<Func<PrintingDetails, bool>> predicate = GetPredicate(request.Search);

            IPagedList<PrintingDetails> pagedList = await unitOfWork.GetRepository<PrintingDetails>()
                .GetPagedListAsync(predicate,
                    pageIndex: request.PageIndex,
                    pageSize: request.PageSize,
                    cancellationToken: cancellationToken);

            if (pagedList.PageIndex > pagedList.TotalPages)
                pagedList = await unitOfWork.GetRepository<PrintingDetails>()
                    .GetPagedListAsync(pageIndex: 0,
                        pageSize: request.PageSize, cancellationToken: cancellationToken);

            IPagedList<PrintingDetailsViewModel>? mapped = mapper.Map<IPagedList<PrintingDetailsViewModel>>(pagedList);

            if (mapped is not null)
                return Operation.Result(mapped);

            return Operation.Error(AppData.Exceptions.MappingException);
        }

        private Expression<Func<PrintingDetails, bool>> GetPredicate(string? search)
        {
            Expression<Func<PrintingDetails, bool>>? predicate = PredicateBuilder.True<PrintingDetails>();

            if (search is null)
                return predicate;

            // predicate = predicate.And(x => x.Message.Contains(search));
            // predicate = predicate.Or(x => x.Logger.Contains(search));
            return predicate;
        }
    }

    public record Request(int PageIndex, int PageSize, string? Search) : IRequest<Operation<IPagedList<PrintingDetailsViewModel>, string>>;
}