using Calabonga.UnitOfWork;
using PrintHub.Domain.Base;
using PrintHub.WPF.Endpoints.ColorEndpoints.ViewModels;

namespace PrintHub.WPF.Endpoints.ColorEndpoints.Queries;

public sealed class UpdateColor
{
    public class Handler(IUnitOfWork unitOfWork, IMapper mapper)
        : IRequestHandler<Request, Operation<ColorViewModel, string>>
    {
        public async Task<Operation<ColorViewModel, string>> Handle(Request colorRequest, CancellationToken cancellationToken)
        {
            IRepository<Color> repository = unitOfWork.GetRepository<Color>();

            Color? entity = await repository.GetFirstOrDefaultAsync(predicate: color => color.Id == colorRequest.Id, disableTracking: false);

            if (entity == null)
                return Operation.Error(AppData.Exceptions.NotFoundException);

            mapper.Map(colorRequest.Model, entity);

            repository.Update(entity);
            await unitOfWork.SaveChangesAsync();

            SaveChangesResult lastResult = unitOfWork.LastSaveChangesResult;

            if (lastResult.IsOk)
            {
                ColorViewModel? mapped = mapper.Map<Color, ColorViewModel>(entity);

                if (mapped is not null)
                    return Operation.Result(mapped);

                return Operation.Error(AppData.Exceptions.MappingException);
            }

            string errorMessage = lastResult.Exception?.Message ?? "Something went wrong";
            return Operation.Error(errorMessage);
        }
    }

    public record Request(Guid Id, ColorUpdateViewModel Model) : IRequest<Operation<ColorViewModel, string>>;
}