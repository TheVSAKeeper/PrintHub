﻿using Calabonga.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using PrintHub.Domain.Base;
using PrintHub.WPF.Endpoints.OrderEndpoints.ViewModels;

namespace PrintHub.WPF.Endpoints.OrderEndpoints.Queries;

public sealed class GetOrderById
{
    public class Handler(IUnitOfWork unitOfWork, IMapper mapper)
        : IRequestHandler<Request, Operation<OrderViewModel, string>>
    {
        public async Task<Operation<OrderViewModel, string>> Handle(Request request, CancellationToken cancellationToken)
        {
            Guid id = request.Id;
            IRepository<Order> repository = unitOfWork.GetRepository<Order>();

            Order? entityWithoutIncludes = await repository.GetFirstOrDefaultAsync(predicate: order => order.Id == id,
                include: i => i.Include(x => x.Items)!);

            if (entityWithoutIncludes == null)
                return Operation.Error($"Entity with identifier {id} not found");

            OrderViewModel? mapped = mapper.Map<OrderViewModel>(entityWithoutIncludes);

            if (mapped is not null)
                return Operation.Result(mapped);

            return Operation.Error(AppData.Exceptions.MappingException);
        }
    }

    public record Request(Guid Id) : IRequest<Operation<OrderViewModel, string>>;
}