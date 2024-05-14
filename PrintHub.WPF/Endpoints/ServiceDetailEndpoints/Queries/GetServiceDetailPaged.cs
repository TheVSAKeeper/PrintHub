using System.Linq.Expressions;
using AutoMapper;
using Calabonga.PagedListCore;
using Calabonga.PredicatesBuilder;
using Calabonga.Results;
using Calabonga.UnitOfWork;
using MediatR;
using PrintHub.Domain;
using PrintHub.Domain.Base;
using PrintHub.WPF.Endpoints.ServiceDetailEndpoints.ViewModels;

namespace PrintHub.WPF.Endpoints.ServiceDetailEndpoints.Queries;

public sealed class GetServiceDetailPaged
{
    public class Handler(IUnitOfWork unitOfWork, IMapper mapper)
        : IRequestHandler<Request, Operation<IPagedList<ServiceDetailViewModel>, string>>
    {
        public async Task<Operation<IPagedList<ServiceDetailViewModel>, string>> Handle(
            Request request,
            CancellationToken cancellationToken)
        {
            Expression<Func<ServiceDetail, bool>> predicate = GetPredicate(request.Search);

            IPagedList<ServiceDetail> pagedList = await unitOfWork.GetRepository<ServiceDetail>()
                .GetPagedListAsync(predicate,
                    pageIndex: request.PageIndex,
                    pageSize: request.PageSize,
                    cancellationToken: cancellationToken);

            if (pagedList.PageIndex > pagedList.TotalPages)
                pagedList = await unitOfWork.GetRepository<ServiceDetail>()
                    .GetPagedListAsync(pageIndex: 0,
                        pageSize: request.PageSize, cancellationToken: cancellationToken);

            IPagedList<ServiceDetailViewModel>? mapped = mapper.Map<IPagedList<ServiceDetailViewModel>>(pagedList);

            if (mapped is not null)
                return Operation.Result(mapped);

            return Operation.Error(AppData.Exceptions.MappingException);
        }

        private Expression<Func<ServiceDetail, bool>> GetPredicate(string? search)
        {
            Expression<Func<ServiceDetail, bool>>? predicate = PredicateBuilder.True<ServiceDetail>();

            if (search is null)
                return predicate;

            // predicate = predicate.And(x => x.Message.Contains(search));
            // predicate = predicate.Or(x => x.Logger.Contains(search));
            return predicate;
        }
    }

    public record Request(int PageIndex, int PageSize, string? Search) : IRequest<Operation<IPagedList<ServiceDetailViewModel>, string>>;
}