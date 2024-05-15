using FluentValidation;

namespace PrintHub.WPF.Endpoints.OrderEndpoints.Update;

public class OrderUpdateRequestValidator : AbstractValidator<OrderUpdateFormViewModel>
{
    public OrderUpdateRequestValidator()
    {
        RuleSet("default", () =>
        {
            RuleFor(request => request.Description).NotNull().Length(10, 1024);
        });
    }
}