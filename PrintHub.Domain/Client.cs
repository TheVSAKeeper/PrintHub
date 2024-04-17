using PrintHub.Domain.Base;

namespace PrintHub.Domain;

public class Client : Identity
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Address { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }

    public List<Order> Orders { get; set; }
}