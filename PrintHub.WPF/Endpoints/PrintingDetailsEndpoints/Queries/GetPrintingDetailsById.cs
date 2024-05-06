using AutoMapper;
using Calabonga.Results;
using Calabonga.UnitOfWork;
using MediatR;
using PrintHub.Domain;
using PrintHub.Domain.Base;
using PrintHub.WPF.Endpoints.PrintingDetailsEndpoints.ViewModels;

namespace PrintHub.WPF.Endpoints.PrintingDetailsEndpoints.Queries;

public sealed class GetPrintingDetailsById
{
    public class Handler(IUnitOfWork unitOfWork, IMapper mapper)
        : IRequestHandler<Request, Operation<PrintingDetailsViewModel, string>>
    {
        public async Task<Operation<PrintingDetailsViewModel, string>> Handle(Request request, CancellationToken cancellationToken)
        {
            Guid id = request.Id;
            IRepository<PrintingDetails> repository = unitOfWork.GetRepository<PrintingDetails>();
            PrintingDetails? entityWithoutIncludes = await repository.GetFirstOrDefaultAsync(predicate: PrintingDetails => PrintingDetails.Id == id);

            if (entityWithoutIncludes == null)
                return Operation.Error($"Entity with identifier {id} not found");

            PrintingDetailsViewModel? mapped = mapper.Map<PrintingDetailsViewModel>(entityWithoutIncludes);

            if (mapped is not null)
                return Operation.Result(mapped);

            return Operation.Error(AppData.Exceptions.MappingException);
        }
    }

    public record Request(Guid Id) : IRequest<Operation<PrintingDetailsViewModel, string>>;
}