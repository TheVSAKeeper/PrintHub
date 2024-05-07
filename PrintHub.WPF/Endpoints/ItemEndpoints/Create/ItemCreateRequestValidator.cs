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
        });
    }
}