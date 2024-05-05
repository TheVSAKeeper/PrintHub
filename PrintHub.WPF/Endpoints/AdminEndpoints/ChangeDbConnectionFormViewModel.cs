using System.Windows;
using System.Windows.Input;
using Npgsql;
using PrintHub.Domain;
using PrintHub.WPF.Shared.Commands;
using PrintHub.WPF.Shared.ViewModels;

namespace PrintHub.WPF.Endpoints.AdminEndpoints;

public class ChangeDbConnectionFormViewModel : ViewModelBase
{
    private ICommand? _createConnectionStringCommand;
    private ICommand? _resetCommand;
    private ICommand? _restartCommand;

    private string? _connectionString;
    private string? _database;
    private string? _host;
    private string? _password;
    private string? _username;

    public ChangeDbConnectionFormViewModel()
    {
        FillConnectionData(App.GetConnectionString());
    }

    public string? Host
    {
        get => _host;
        set
        {
            Set(ref _host, value);

            _host = value;
            OnPropertyChanged();
        }
    }

    public string? Username
    {
        get => _username;
        set => Set(ref _username, value);
    }

    public string? Password
    {
        get => _password;
        set => Set(ref _password, value);
    }

    public string? Database
    {
        get => _database;
        set => Set(ref _database, value);
    }

    public string? ConnectionString
    {
        get => _connectionString;
        set => Set(ref _connectionString, value);
    }

    public ICommand RestartCommand => _restartCommand ??= new LambdaCommand(() =>
    {
        MessageBoxResult boxResult = MessageBox.Show("Are you sure you restart application?", "Restart", MessageBoxButton.OKCancel);

        if (boxResult == MessageBoxResult.OK)
            App.RestartApplication();
    }, () => string.IsNullOrEmpty(ConnectionString) == false);

    public ICommand ResetCommand => _resetCommand ??= new LambdaCommand(() =>
    {
        App.SetConnectionString(string.Empty);
        ConnectionString = App.GetConnectionString();
    });

    public ICommand CreateConnectionStringCommand => _createConnectionStringCommand ??= new LambdaCommand(() =>
    {
        NpgsqlConnectionStringBuilder builder = new()
        {
            Host = Host,
            Username = Username,
            Password = Password,
            Database = Database
        };

        if (IsDatabaseStructureValid(builder.ConnectionString) == false)
        {
            MessageBox.Show("Invalid connection string, database does not exist, "
                            + "or database structure does not match the expected structure. Please check the values.", "Error");

            return;
        }

        ConnectionString = builder.ConnectionString;
        App.SetConnectionString(ConnectionString);
    });

    private void FillConnectionData(string connectionString)
    {
        NpgsqlConnectionStringBuilder builder = new(connectionString);
        Host = builder.Host;
        Username = builder.Username;
        Password = builder.Password;
        Database = builder.Database;
    }

    private static bool IsDatabaseStructureValid(string connectionString)
    {
        try
        {
            using NpgsqlConnection connection = new(connectionString);

            connection.Open();

            const string Query = $"SELECT EXISTS (SELECT FROM information_schema.tables WHERE table_name = '{nameof(PrintingDetails)}')";

            using NpgsqlCommand command = new(Query, connection);

            bool tableExists = (bool)(command.ExecuteScalar() ?? false);
            return tableExists;
        }
        catch
        {
            return false;
        }
    }
}