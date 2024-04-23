using FluentValidation;
using PrintHub.WPF.Endpoints.ColorEndpoints.Queries;

namespace PrintHub.WPF.Endpoints.ColorEndpoints;

public class ColorCreateRequestValidator : AbstractValidator<CreateColor.Request>
{
    public ColorCreateRequestValidator()
    {
        RuleSet("default", () =>
        {
            //  RuleFor(request => request.Model.CreatedAt).NotNull();
        });
    }
}