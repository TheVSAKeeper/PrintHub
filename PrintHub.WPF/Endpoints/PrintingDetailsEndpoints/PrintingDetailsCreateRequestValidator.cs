using FluentValidation;
using PrintHub.WPF.Endpoints.PrintingDetailsEndpoints.Queries;

namespace PrintHub.WPF.Endpoints.PrintingDetailsEndpoints;

public class PrintingDetailsCreateRequestValidator : AbstractValidator<CreatePrintingDetails.Request>
{
    public PrintingDetailsCreateRequestValidator()
    {
        RuleSet("default", () =>
        {
            //  RuleFor(request => request.Model.CreatedAt).NotNull();
        });
    }
}