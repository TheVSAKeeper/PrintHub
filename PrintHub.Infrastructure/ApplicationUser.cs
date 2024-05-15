using Microsoft.AspNetCore.Identity;
using PrintHub.Domain;

namespace PrintHub.Infrastructure;

/// <summary>
///     Пользователь по умолчанию для приложения.
///     Добавьте данные профиля для пользователей приложения, добавив свойства в класс ApplicationUser
/// </summary>
public class ApplicationUser : IdentityUser<Guid>
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }

    public Guid? ClientId { get; set; }
    public Client? Client { get; set; }

    public virtual List<ApplicationRole>? Roles { get; set; }
}