using FluentValidation;
using PrintHub.WPF.Shared;

namespace PrintHub.WPF.Endpoints.PrintingDetailsEndpoints.Create;

public class PrintingDetailsCreateRequestValidator : AbstractValidator<PrintingDetailsCreateFormViewModel>
{
    public PrintingDetailsCreateRequestValidator()
    {
        RuleSet("default", () =>
        {
            RuleFor(request => request.Description).NotNull().Length(10, 1024);

            RuleFor(request => request.AllMaterials)
                .Must(HaveExactlyOneCheckedElement)
                .WithMessage("ChosenMaterials must contain exactly one checked element.");

            RuleFor(request => request.AllColors)
                .Must(HaveExactlyOneCheckedElement)
                .WithMessage("ChosenColors must contain exactly one checked element.");
        });
    }

    private bool HaveExactlyOneCheckedElement(IEnumerable<Checkable> checkables)
    {
        return checkables.Count(x => x.IsChecked) == 1;
    }
}