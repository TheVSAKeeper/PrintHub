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
            RuleFor(request => request.DevelopmentCost).NotNull().InclusiveBetween(0, 1000);
            RuleFor(request => request.Weight).NotNull().InclusiveBetween(0, 1000000);
        });
    }
}