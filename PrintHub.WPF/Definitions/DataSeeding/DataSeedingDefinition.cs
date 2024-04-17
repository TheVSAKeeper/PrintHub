using Microsoft.Extensions.Hosting;
using PrintHub.Infrastructure.DatabaseInitialization;
using PrintHub.WPF.Definitions.Base;

namespace PrintHub.WPF.Definitions.DataSeeding;

public class DataSeedingDefinition : AppDefinition
{
    public override int OrderIndex => 1;

    public override async Task ConfigureApplication(IHost host)
    {
        const string DataPath = @"Definitions\DataSeeding\data\";

        DatabaseInitializer initializer = new(host.Services, DataPath);

        await initializer.SeedUsers();
        await initializer.SeedDiagnoses();
        await initializer.SeedAnamnesisTemplates();
        await initializer.SeedPatients();
    }
}