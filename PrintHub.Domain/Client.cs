using PrintHub.Domain.Base;

namespace PrintHub.Domain;

public class Client : Identity
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Address { get; set; }
    public required string Phone { get; set; }
    public required string Email { get; set; }

    public virtual List<Order>? Orders { get; set; }
}