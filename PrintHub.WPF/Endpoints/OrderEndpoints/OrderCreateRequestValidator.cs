using FluentValidation;
using PrintHub.WPF.Endpoints.OrderEndpoints.Queries;

namespace PrintHub.WPF.Endpoints.OrderEndpoints;

public class OrderCreateRequestValidator : AbstractValidator<CreateOrder.Request>
{
    public OrderCreateRequestValidator()
    {
        RuleSet("default", () =>
        {
            //  RuleFor(request => request.Model.CreatedAt).NotNull();
        });
    }
}