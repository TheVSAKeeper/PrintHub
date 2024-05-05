using Calabonga.Results;

namespace PrintHub.WPF.Endpoints.AdminEndpoints;

public interface IDbConnectionService
{
    string GetConnectionString(bool isDefault = false);
    void SetConnectionString(string connectionString);
    Operation<bool, string> IsDatabaseValid(string connectionString);
}