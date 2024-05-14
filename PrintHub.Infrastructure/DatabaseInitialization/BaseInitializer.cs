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
                CreatedBy = "Superuser",
                Weight = 22.5m,
                DevelopmentCost = 65.7m
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
                CreatedBy = "Superuser",
                Weight = 55.6m,
                DevelopmentCost = 12.3m
            }
        ];

        await _context.Items.AddRangeAsync(items);

        List<AdditionalService> additionalServices =
        [
            new AdditionalService()
            {
                Name = "Сканирование объектов",
                Description = "Мы можем отсканировать ваш физический объект и создать его 3D-модель для последующей печати.",
                ServiceDetails = new List<ServiceDetail>
                {
                    new()
                    {
                        Name = "Лазерное сканирование",
                        Description = "Высокоточное лазерное сканирование объектов с точностью до 0,1 мм.",
                        Price = 5.7m
                    },
                    new()
                    {
                        Name = "Фотограмметрическое сканирование",
                        Description = "Создание 3D-модели на основе серии фотографий объекта.",
                        Price = 6.5m
                    }
                }
            },

            new AdditionalService()
            {
                Name = "Моделирование и дизайн",
                Description = "Наши специалисты помогут вам разработать 3D-модель, если у вас нет готового файла для печати.",
                ServiceDetails = new List<ServiceDetail>
                {
                    new()
                    {
                        Name = "3D-моделирование",
                        Description = "Создание 3D-модели по вашим эскизам или техническим требованиям.",
                        Price = 7.5m
                    },
                    new()
                    {
                        Name = "Дизайн-консультации",
                        Description = "Помощь в разработке дизайна и концепции вашего изделия.",
                        Price = 4.1m
                    }
                }
            },

            new AdditionalService()
            {
                Name = "Постобработка",
                Description = "Мы можем обработать напечатанные изделия, например, отшлифовать, покрасить или нанести дополнительные покрытия.",
                ServiceDetails = new List<ServiceDetail>
                {
                    new()
                    {
                        Name = "Шлифовка и полировка",
                        Description = "Ручная или механическая обработка поверхности для достижения гладкого финиша.",
                        Price = 5.7m
                    },
                    new()
                    {
                        Name = "Покраска",
                        Description = "Нанесение краски или лака для придания изделию желаемого цвета и защитного покрытия.",
                        Price = 3.5m
                    },
                    new()
                    {
                        Name = "Нанесение покрытий",
                        Description = "Нанесение специальных покрытий, таких как металлизация, гальваника или лакировка.",
                        Price = 9.1m
                    }
                }
            },

            new AdditionalService()
            {
                Name = "Консультации",
                Description = "Наши эксперты готовы проконсультировать вас по вопросам 3D-печати, выбора материалов и технологий.",
                ServiceDetails = new List<ServiceDetail>
                {
                    new()
                    {
                        Name = "Консультация по 3D-печати",
                        Description = "Помощь в выборе оптимальной технологии и материалов для вашего проекта.",
                        Price = 5.9m
                    },
                    new()
                    {
                        Name = "Консультация по дизайну",
                        Description = "Рекомендации по улучшению дизайна и оптимизации 3D-модели для печати.",
                        Price = 3.9m
                    },
                    new()
                    {
                        Name = "Консультация по постобработке",
                        Description = "Советы по выбору методов и материалов для постобработки напечатанных изделий.",
                        Price = 4.8m
                    }
                }
            }
        ];

        await _context.AdditionalServices.AddRangeAsync(additionalServices);

        await _context.SaveChangesAsync();
    }
}