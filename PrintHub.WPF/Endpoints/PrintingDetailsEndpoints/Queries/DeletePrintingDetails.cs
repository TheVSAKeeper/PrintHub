using AutoMapper;
using Calabonga.Results;
using Calabonga.UnitOfWork;
using MediatR;
using PrintHub.Domain;
using PrintHub.Domain.Base;
using PrintHub.WPF.Endpoints.PrintingDetailsEndpoints.ViewModels;

namespace PrintHub.WPF.Endpoints.PrintingDetailsEndpoints.Queries;

public sealed class DeletePrintingDetails
{
    public class Handler(IUnitOfWork unitOfWork, IMapper mapper)
        : IRequestHandler<Request, Operation<PrintingDetailsViewModel, string>>
    {
        public async Task<Operation<PrintingDetailsViewModel, string>> Handle(Request request, CancellationToken cancellationToken)
        {
            IRepository<PrintingDetails> repository = unitOfWork.GetRepository<PrintingDetails>();
            PrintingDetails? entity = await repository.FindAsync([request.Id], cancellationToken);

            if (entity == null)
                return Operation.Error("Entity not found");

            repository.Delete(entity);
            await unitOfWork.SaveChangesAsync();

            if (unitOfWork.LastSaveChangesResult.IsOk == false)
                return Operation.Error(unitOfWork.LastSaveChangesResult.Exception?.Message ?? AppData.Exceptions.SomethingWrong);

            PrintingDetailsViewModel? mapped = mapper.Map<PrintingDetailsViewModel>(entity);

            if (mapped is not null)
                return Operation.Result(mapped);

            return Operation.Error(AppData.Exceptions.MappingException);
        }
    }

    public record Request(Guid Id) : IRequest<Operation<PrintingDetailsViewModel, string>>;
}