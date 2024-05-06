using System.Linq.Expressions;
using AutoMapper;
using Calabonga.PagedListCore;
using Calabonga.PredicatesBuilder;
using Calabonga.Results;
using Calabonga.UnitOfWork;
using MediatR;
using PrintHub.Domain;
using PrintHub.Domain.Base;
using PrintHub.WPF.Endpoints.ItemEndpoints.ViewModels;

namespace PrintHub.WPF.Endpoints.ItemEndpoints.Queries;

public sealed class GetItemPaged
{
    public class Handler(IUnitOfWork unitOfWork, IMapper mapper)
        : IRequestHandler<Request, Operation<IPagedList<ItemViewModel>, string>>
    {
        public async Task<Operation<IPagedList<ItemViewModel>, string>> Handle(
            Request request,
            CancellationToken cancellationToken)
        {
            Expression<Func<Item, bool>> predicate = GetPredicate(request.Search);

            IPagedList<Item> pagedList = await unitOfWork.GetRepository<Item>()
                .GetPagedListAsync(predicate,
                    pageIndex: request.PageIndex,
                    pageSize: request.PageSize,
                    cancellationToken: cancellationToken);

            if (pagedList.PageIndex > pagedList.TotalPages)
                pagedList = await unitOfWork.GetRepository<Item>()
                    .GetPagedListAsync(pageIndex: 0,
                        pageSize: request.PageSize, cancellationToken: cancellationToken);

            IPagedList<ItemViewModel>? mapped = mapper.Map<IPagedList<ItemViewModel>>(pagedList);

            if (mapped is not null)
                return Operation.Result(mapped);

            return Operation.Error(AppData.Exceptions.MappingException);
        }

        private Expression<Func<Item, bool>> GetPredicate(string? search)
        {
            Expression<Func<Item, bool>>? predicate = PredicateBuilder.True<Item>();

            if (search is null)
                return predicate;

            // predicate = predicate.And(x => x.Message.Contains(search));
            // predicate = predicate.Or(x => x.Logger.Contains(search));
            return predicate;
        }
    }

    public record Request(int PageIndex, int PageSize, string? Search) : IRequest<Operation<IPagedList<ItemViewModel>, string>>;
}