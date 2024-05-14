using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using PrintHub.Domain;
using PrintHub.Infrastructure.Base;

namespace PrintHub.Infrastructure;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContextBase(options)
{
    public DbSet<Client> Clients { get; set; } = null!;
    public DbSet<Color> Colors { get; set; } = null!;
    public DbSet<Item> Items { get; set; } = null!;
    public DbSet<Material> Materials { get; set; } = null!;
    public DbSet<Order> Orders { get; set; } = null!;
    public DbSet<PrintingDetails> PrintingDetails { get; set; } = null!;
    public DbSet<Sample> Samples { get; set; } = null!;
    public DbSet<AdditionalService> AdditionalServices { get; set; } = null!;
    public DbSet<ServiceDetail> ServiceDetails { get; set; } = null!;
}

public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        DbContextOptionsBuilder<ApplicationDbContext> optionsBuilder = new();
        optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=print_hub;Username=postgres;");
        return new ApplicationDbContext(optionsBuilder.Options);
    }
}