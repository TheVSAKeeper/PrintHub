using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using PrintHub.Domain;
using PrintHub.Infrastructure.Base;

namespace PrintHub.Infrastructure;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContextBase(options)
{
    public DbSet<Client> Clients { get; set; } = null!;
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