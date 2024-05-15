using FluentValidation;

namespace PrintHub.WPF.Endpoints.ItemEndpoints.Create;

public class ItemCreateRequestValidator : AbstractValidator<ItemCreateFormViewModel>
{
    public ItemCreateRequestValidator()
    {
        RuleSet("default", () =>
        {
            RuleFor(request => request.Description).NotNull().Length(10, 1024);
            RuleFor(request => request.PrintingDetails).NotNull();

            RuleFor(request => request.DevelopmentCost).NotEmpty().InclusiveBetween(0, 1000);
            RuleFor(request => request.Weight).NotEmpty().InclusiveBetween(0, 1000000);
        });
    }
}