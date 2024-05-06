using FluentValidation;
using PrintHub.WPF.Endpoints.ItemEndpoints.Queries;

namespace PrintHub.WPF.Endpoints.ItemEndpoints;

public class ItemCreateRequestValidator : AbstractValidator<CreateItem.Request>
{
    public ItemCreateRequestValidator()
    {
        RuleSet("default", () =>
        {
            //  RuleFor(request => request.Model.CreatedAt).NotNull();
        });
    }
}