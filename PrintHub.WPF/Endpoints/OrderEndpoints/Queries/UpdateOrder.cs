﻿using Calabonga.UnitOfWork;
using PrintHub.Domain.Base;
using PrintHub.Infrastructure;
using PrintHub.WPF.Endpoints.AuthenticationEndpoints;
using PrintHub.WPF.Endpoints.OrderEndpoints.ViewModels;

namespace PrintHub.WPF.Endpoints.OrderEndpoints.Queries;

public sealed class UpdateOrder
{
    public class Handler(IUnitOfWork unitOfWork, IMapper mapper, AuthenticationStore authenticationStore)
        : IRequestHandler<Request, Operation<OrderViewModel, string>>
    {
        public async Task<Operation<OrderViewModel, string>> Handle(Request orderRequest, CancellationToken cancellationToken)
        {
            IRepository<Order> repository = unitOfWork.GetRepository<Order>();

            Order? entity = await repository.GetFirstOrDefaultAsync(predicate: order => order.Id == orderRequest.Id, disableTracking: false);

            if (entity == null)
                return Operation.Error(AppData.Exceptions.NotFoundException);

            mapper.Map(orderRequest.Model, entity, options => options.Items[nameof(ApplicationUser)] = authenticationStore.User?.UserName);

            repository.Update(entity);
            await unitOfWork.SaveChangesAsync();

            SaveChangesResult lastResult = unitOfWork.LastSaveChangesResult;

            if (lastResult.IsOk)
            {
                OrderViewModel? mapped = mapper.Map<Order, OrderViewModel>(entity);

                if (mapped is not null)
                    return Operation.Result(mapped);

                return Operation.Error(AppData.Exceptions.MappingException);
            }

            string errorMessage = lastResult.Exception?.Message ?? "Something went wrong";
            return Operation.Error(errorMessage);
        }
    }

    public record Request(Guid Id, OrderUpdateViewModel Model) : IRequest<Operation<OrderViewModel, string>>;
}