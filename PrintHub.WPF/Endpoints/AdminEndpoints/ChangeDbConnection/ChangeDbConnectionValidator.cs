using FluentValidation;

namespace PrintHub.WPF.Endpoints.AdminEndpoints.ChangeDbConnection;

public class ChangeDbConnectionValidator : AbstractValidator<ChangeDbConnectionFormViewModel>
{
    public ChangeDbConnectionValidator()
    {
        RuleSet("default", () =>
        {
            RuleFor(x => x.Database)
                .NotEmpty()
                .Length(3, 63)
                .Matches("^[a-zA-Z_][a-zA-Z0-9_]*$")
                .WithMessage("Database name must be a valid PostgreSQL identifier (up to 63 characters, starting with a letter or underscore, and containing only letters, digits, and underscores).")
                ;

            RuleFor(x => x.Host)
                .NotEmpty()
                .Length(3, 255)
                .Matches("^[a-zA-Z0-9.-]+$")
                .WithMessage("Host must be a valid hostname or IP address (up to 255 characters, containing only letters, digits, dots, and hyphens).");

            RuleFor(x => x.Username)
                .NotEmpty()
                .Length(3, 63)
                .Matches("^[a-zA-Z_][a-zA-Z0-9_]*$")
                .WithMessage("Username must be a valid PostgreSQL identifier (up to 63 characters, starting with a letter or underscore, and containing only letters, digits, and underscores).");
        });
    }
}