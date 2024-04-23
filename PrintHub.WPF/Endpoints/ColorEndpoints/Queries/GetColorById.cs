using AutoMapper;
using Calabonga.Results;
using Calabonga.UnitOfWork;
using MediatR;
using PrintHub.Domain;
using PrintHub.Domain.Base;
using PrintHub.WPF.Endpoints.ColorEndpoints.ViewModels;

namespace PrintHub.WPF.Endpoints.ColorEndpoints.Queries;

public sealed class GetColorById
{
    public class Handler(IUnitOfWork unitOfWork, IMapper mapper)
        : IRequestHandler<Request, Operation<ColorViewModel, string>>
    {
        public async Task<Operation<ColorViewModel, string>> Handle(Request request, CancellationToken cancellationToken)
        {
            Guid id = request.Id;
            IRepository<Color> repository = unitOfWork.GetRepository<Color>();
            Color? entityWithoutIncludes = await repository.GetFirstOrDefaultAsync(predicate: color => color.Id == id);

            if (entityWithoutIncludes == null)
                return Operation.Error($"Entity with identifier {id} not found");

            ColorViewModel? mapped = mapper.Map<ColorViewModel>(entityWithoutIncludes);

            if (mapped is not null)
                return Operation.Result(mapped);

            return Operation.Error(AppData.Exceptions.MappingException);
        }
    }

    public record Request(Guid Id) : IRequest<Operation<ColorViewModel, string>>;
}