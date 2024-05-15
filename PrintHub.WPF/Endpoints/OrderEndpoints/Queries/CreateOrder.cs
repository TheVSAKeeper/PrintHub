using Calabonga.UnitOfWork;
using Microsoft.Extensions.Logging;
using PrintHub.Domain.Base;
using PrintHub.Infrastructure;
using PrintHub.WPF.Endpoints.OrderEndpoints.ViewModels;

namespace PrintHub.WPF.Endpoints.OrderEndpoints.Queries;

public sealed class CreateOrder
{
    public class Handler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<Handler> logger)
        : IRequestHandler<Request, Operation<OrderViewModel, string>>
    {
        public async Task<Operation<OrderViewModel, string>> Handle(Request orderRequest, CancellationToken cancellationToken)
        {
            logger.LogDebug("Creating new Order");

            Order? entity = mapper.Map<OrderCreateViewModel, Order>(orderRequest.Model,
                options => options.Items[nameof(ApplicationUser)] = orderRequest.User.UserName);

            if (entity == null)
            {
                logger.LogError("Mapper not configured correctly or something went wrong");
                return Operation.Error(AppData.Exceptions.MappingException);
            }

            Guid[] chosenColorsKeys = orderRequest.Model.RequiredColors.Select(x => x.Id).ToArray();

            IList<Color> chosenColors = await unitOfWork.GetRepository<Color>()
                .GetAllAsync(predicate: color => chosenColorsKeys.Contains(color.Id), disableTracking: false);

            entity.RequiredColors = chosenColors.ToList();

            Guid[] chosenServicesKeys = orderRequest.Model.ServiceDetails!.Select(x => x.Id).ToArray();

            IList<ServiceDetail> chosenServices = await unitOfWork.GetRepository<ServiceDetail>()
                .GetAllAsync(predicate: serviceDetail => chosenServicesKeys.Contains(serviceDetail.Id), disableTracking: false);

            entity.ServiceDetails = chosenServices.ToList();

            await unitOfWork.GetRepository<Order>().InsertAsync(entity, cancellationToken);
            await unitOfWork.SaveChangesAsync();

            SaveChangesResult lastResult = unitOfWork.LastSaveChangesResult;

            if (lastResult.IsOk)
            {
                OrderViewModel? mapped = mapper.Map<Order, OrderViewModel>(entity);

                if (mapped is null)
                    return Operation.Error(AppData.Exceptions.MappingException);

                logger.LogInformation("New entity {@Order} successfully created", entity);
                return Operation.Result(mapped);
            }

            string errorMessage = lastResult.Exception?.Message ?? "Something went wrong";
            logger.LogError(errorMessage);
            return Operation.Error(errorMessage);
        }
    }

    public record Request(OrderCreateViewModel Model, ApplicationUser User) : IRequest<Operation<OrderViewModel, string>>;
}