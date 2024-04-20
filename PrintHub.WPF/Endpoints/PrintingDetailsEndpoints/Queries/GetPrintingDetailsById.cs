using AutoMapper;
using Calabonga.Results;
using Calabonga.UnitOfWork;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PrintHub.Domain;
using PrintHub.Domain.Base;

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

            PrintingDetails? entity = await repository.GetFirstOrDefaultAsync(predicate: x => x.Id == id,
                include: i => i.Include(details => details.Color)
                    .Include(details => details.Material)
                    .ThenInclude(material => material!.AvailableColors));

            if (entity == null)
                return Operation.Error($"Entity with identifier {id} not found");

            PrintingDetailsViewModel? mapped = mapper.Map<PrintingDetailsViewModel>(entity);

            if (mapped is not null)
                return Operation.Result(mapped);

            return Operation.Error(AppData.Exceptions.MappingException);
        }
    }

    public record Request(Guid Id) : IRequest<Operation<PrintingDetailsViewModel, string>>;
}