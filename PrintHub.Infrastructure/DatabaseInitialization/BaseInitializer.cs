using PrintHub.Domain;

namespace PrintHub.Infrastructure.DatabaseInitialization;

public partial class DatabaseInitializer
{
    public async Task SeedAll()
    {
        if (_context.Clients.Any())
            return;

        List<Client> clients =
        [
            new Client
            {
                Id = Guid.NewGuid(),
                FirstName = "Иван",
                LastName = "Петров",
                Address = "ул. Ленина, д. 10",
                Phone = "8 (800) 123-45-67",
                Email = "ivan.petrov@example.com",
                Orders = new List<Order>()
            },

            new Client
            {
                Id = Guid.NewGuid(),
                FirstName = "Мария",
                LastName = "Сидорова",
                Address = "пр. Победы, д. 20",
                Phone = "8 (800) 765-43-21",
                Email = "maria.sidorova@example.com",
                Orders = new List<Order>()
            }
        ];

        await _context.Clients.AddRangeAsync(clients);

        List<Color> colors =
        [
            new Color
            {
                Id = Guid.NewGuid(),
                Name = "Красный",
                ColorCode = "#FF0000",
                Materials = new List<Material>(),
                PrintingDetails = new List<PrintingDetails>()
            },

            new Color
            {
                Id = Guid.NewGuid(),
                Name = "Синий",
                ColorCode = "#0000FF",
                Materials = new List<Material>(),
                PrintingDetails = new List<PrintingDetails>()
            }
        ];

        await _context.Colors.AddRangeAsync(colors);

        List<Material> materials =
        [
            new Material
            {
                Id = Guid.NewGuid(),
                Name = "PLA",
                Description = "Биоразлагаемый пластик PLA",
                Price = 10.99m,
                AvailableColors = [colors[0], colors[1]],
                PrintingDetails = new List<PrintingDetails>(),
                Technology = PrintingTechnology.FDM
            },

            new Material
            {
                Id = Guid.NewGuid(),
                Name = "PETG",
                Description = "Прочный пластик PETG",
                Price = 15.99m,
                AvailableColors = [colors[1]],
                PrintingDetails = new List<PrintingDetails>(),
                Technology = PrintingTechnology.FDM
            }
        ];

        await _context.Materials.AddRangeAsync(materials);

        List<Order> orders =
        [
            new Order
            {
                Id = Guid.NewGuid(),
                Description = "Первый заказ",
                Status = OrderStatus.New,
                ClientId = clients[0].Id,
                Client = clients[0],
                Samples = new List<Sample>(),
                Items = new List<Item>(),
                RequiredColors = [colors[0]]
            },

            new Order
            {
                Id = Guid.NewGuid(),
                Description = "Completed order",
                Status = OrderStatus.Completed,
                ClientId = clients[0].Id,
                Client = clients[0],
                Samples = new List<Sample>(),
                Items = new List<Item>(),
                RequiredColors = [colors[0]]
            },

            new Order
            {
                Id = Guid.NewGuid(),
                Description = "Второй заказ",
                Status = OrderStatus.InProgress,
                ClientId = clients[1].Id,
                Client = clients[1],
                Samples = new List<Sample>(),
                Items = new List<Item>(),
                RequiredColors = [colors[1]]
            },

            new Order
            {
                Id = Guid.NewGuid(),
                Description = "Третий заказ",
                Status = OrderStatus.InProgress,
                ClientId = clients[0].Id,
                Client = clients[0],
                Samples = new List<Sample>(),
                Items = new List<Item>(),
                RequiredColors = [colors[0], colors[1]]
            }
        ];

        await _context.Orders.AddRangeAsync(orders);

        List<PrintingDetails> printingDetailsList =
        [
            new PrintingDetails
            {
                Id = Guid.NewGuid(),
                Technology = PrintingTechnology.FDM,
                ColorId = colors[0].Id,
                Color = colors[0],
                MaterialId = materials[0].Id,
                Material = materials[0],
                Items = new List<Item>(),
                Samples = new List<Sample>()
            },

            new PrintingDetails
            {
                Id = Guid.NewGuid(),
                Technology = PrintingTechnology.SLA,
                ColorId = colors[1].Id,
                Color = colors[1],
                MaterialId = materials[1].Id,
                Material = materials[1],
                Items = new List<Item>(),
                Samples = new List<Sample>()
            }
        ];

        await _context.PrintingDetails.AddRangeAsync(printingDetailsList);

        List<Sample> samples =
        [
            new Sample
            {
                Id = Guid.NewGuid(),
                Description = "Пример 1",
                Approved = true,
                OrderId = orders[0].Id,
                Order = orders[0],
                PrintingDetailsId = printingDetailsList[0].Id,
                PrintingDetails = printingDetailsList[0]
            },

            new Sample
            {
                Id = Guid.NewGuid(),
                Description = "Пример 2",
                Approved = false,
                OrderId = orders[1].Id,
                Order = orders[1],
                PrintingDetailsId = printingDetailsList[1].Id,
                PrintingDetails = printingDetailsList[1]
            }
        ];

        await _context.Samples.AddRangeAsync(samples);

        List<Item> items =
        [
            new Item
            {
                Description = "Пример описания 1",
                Ready = true,
                OrderId = orders[0].Id,
                Order = orders[0],
                PrintingDetailsId = printingDetailsList[0].Id,
                PrintingDetails = printingDetailsList[0],
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "Superuser"
            },

            new Item
            {
                Description = "Пример описания 2",
                Ready = false,
                OrderId = orders[1].Id,
                Order = orders[1],
                PrintingDetailsId = printingDetailsList[1].Id,
                PrintingDetails = printingDetailsList[1],
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "Superuser"
            }
        ];

        await _context.Items.AddRangeAsync(items);

        await _context.SaveChangesAsync();
    }
}