using System.IO;
using Microsoft.Extensions.Configuration;
using Npgsql;
using PrintHub.Infrastructure;
using PrintHub.WPF.Properties;

namespace PrintHub.WPF.Shared.Services;

public class DbConnectionService : IDbConnectionService
{
    public string GetConnectionString(bool isDefault = false)
    {
        if (isDefault == false && string.IsNullOrEmpty(Settings.Default.ConnectionString) == false)
            return Settings.Default.ConnectionString;

        IConfigurationBuilder configBuilder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json");

        IConfigurationRoot configuration = configBuilder.Build();
        return configuration.GetConnectionString(nameof(ApplicationDbContext)) ?? string.Empty;
    }

    public void SetConnectionString(string connectionString)
    {
        if (IsDatabaseValid(connectionString) == false)
            return;

        Settings.Default.ConnectionString = connectionString;
        Settings.Default.Save();
    }

    public Operation<bool, string> IsDatabaseValid(string connectionString)
    {
        string exception = "Invalid connection string.";

        try
        {
            using NpgsqlConnection connection = new(connectionString);

            exception = "Database does not exist.";
            connection.Open();

            exception = "An unexpected error occurred.";

            using NpgsqlConnection defaultConnection = new(GetConnectionString(true));
            defaultConnection.Open();

            HashSet<string> providedTables = GetTableNames(connection);
            HashSet<string> defaultTables = GetTableNames(defaultConnection);

            HashSet<string> differenceSet = [..providedTables];
            differenceSet.SymmetricExceptWith(defaultTables);
            bool isStructureValid = differenceSet.Count == 0;

            if (isStructureValid)
                return Operation.Result(isStructureValid);

            return Operation.Error($"Database structure does not match the expected structure. "
                                   + $"Tables present in one database but not the other: {string.Join(", ", differenceSet)}");
        }
        catch
        {
            return Operation.Error(exception);
        }
    }

    private static HashSet<string> GetTableNames(NpgsqlConnection connection)
    {
        HashSet<string> tableNames = [];

        using NpgsqlCommand command = new("SELECT table_name FROM information_schema.tables WHERE table_schema = 'public'", connection);
        using NpgsqlDataReader reader = command.ExecuteReader();

        while (reader.Read())
        {
            tableNames.Add(reader.GetString(0));
        }

        return tableNames;
    }
}