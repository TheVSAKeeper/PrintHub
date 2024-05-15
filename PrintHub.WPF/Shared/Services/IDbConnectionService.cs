namespace PrintHub.WPF.Shared.Services;

public interface IDbConnectionService
{
    string GetConnectionString(bool isDefault = false);
    void SetConnectionString(string connectionString);
    Operation<bool, string> IsDatabaseValid(string connectionString);
}