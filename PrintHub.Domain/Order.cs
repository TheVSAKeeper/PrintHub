using PrintHub.Domain.Base;

namespace PrintHub.Domain;

public class Order : Auditable
{
    public string Description { get; set; }
    public OrderStatus Status { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }

    public int ClientId { get; set; }
    public Client Client { get; set; }

    public List<Sample> Samples { get; set; }
    public List<Item> Items { get; set; }
}