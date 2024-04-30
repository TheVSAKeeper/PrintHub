using FluentValidation;

namespace PrintHub.WPF.Endpoints.OrderEndpoints;

public class OrderCreateRequestValidator : AbstractValidator<OrderCreateFormViewModel>
{
    public OrderCreateRequestValidator()
    {
        RuleSet("default", () =>
        {
            RuleFor(request => request.Description).NotNull().Length(10, 1024);
        });
    }
}