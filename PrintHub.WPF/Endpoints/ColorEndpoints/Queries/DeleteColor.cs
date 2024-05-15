using Calabonga.UnitOfWork;
using PrintHub.Domain.Base;
using PrintHub.WPF.Endpoints.ColorEndpoints.ViewModels;

namespace PrintHub.WPF.Endpoints.ColorEndpoints.Queries;

public sealed class DeleteColor
{
    public class Handler(IUnitOfWork unitOfWork, IMapper mapper)
        : IRequestHandler<Request, Operation<ColorViewModel, string>>
    {
        public async Task<Operation<ColorViewModel, string>> Handle(Request request, CancellationToken cancellationToken)
        {
            IRepository<Color> repository = unitOfWork.GetRepository<Color>();
            Color? entity = await repository.FindAsync([request.Id], cancellationToken);

            if (entity == null)
                return Operation.Error("Entity not found");

            repository.Delete(entity);
            await unitOfWork.SaveChangesAsync();

            if (unitOfWork.LastSaveChangesResult.IsOk == false)
                return Operation.Error(unitOfWork.LastSaveChangesResult.Exception?.Message ?? AppData.Exceptions.SomethingWrong);

            ColorViewModel? mapped = mapper.Map<ColorViewModel>(entity);

            if (mapped is not null)
                return Operation.Result(mapped);

            return Operation.Error(AppData.Exceptions.MappingException);
        }
    }

    public record Request(Guid Id) : IRequest<Operation<ColorViewModel, string>>;
}