﻿using System.Linq.Expressions;
using Calabonga.PagedListCore;
using Calabonga.PredicatesBuilder;
using Calabonga.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using PrintHub.Domain.Base;
using PrintHub.WPF.Endpoints.OrderEndpoints.ViewModels;

namespace PrintHub.WPF.Endpoints.OrderEndpoints.Queries;

public sealed class GetOrderPaged
{
    public class Handler(IUnitOfWork unitOfWork, IMapper mapper)
        : IRequestHandler<Request, Operation<IPagedList<OrderViewModel>, string>>
    {
        public async Task<Operation<IPagedList<OrderViewModel>, string>> Handle(
            Request request,
            CancellationToken cancellationToken)
        {
            Expression<Func<Order, bool>> predicate = GetPredicate(request.Search);

            IPagedList<Order> pagedList = await unitOfWork.GetRepository<Order>()
                .GetPagedListAsync(predicate,
                    pageIndex: request.PageIndex,
                    pageSize: request.PageSize,
                    include: i => i.Include(order => order.RequiredColors),
                    cancellationToken: cancellationToken);

            if (pagedList.PageIndex > pagedList.TotalPages)
                pagedList = await unitOfWork.GetRepository<Order>()
                    .GetPagedListAsync(pageIndex: 0,
                        pageSize: request.PageSize,
                        include: i => i.Include(order => order.RequiredColors),
                        cancellationToken: cancellationToken);

            IPagedList<OrderViewModel>? mapped = mapper.Map<IPagedList<OrderViewModel>>(pagedList);

            if (mapped is not null)
                return Operation.Result(mapped);

            return Operation.Error(AppData.Exceptions.MappingException);
        }

        private Expression<Func<Order, bool>> GetPredicate(string? search)
        {
            Expression<Func<Order, bool>>? predicate = PredicateBuilder.True<Order>();

            if (search is null)
                return predicate;

            predicate = predicate.And(x => x.Description.Contains(search));
            predicate = predicate.Or(x => x.ClientId.ToString() == search);

            return predicate;
        }
    }

    public record Request(int PageIndex, int PageSize, string? Search) : IRequest<Operation<IPagedList<OrderViewModel>, string>>;
}