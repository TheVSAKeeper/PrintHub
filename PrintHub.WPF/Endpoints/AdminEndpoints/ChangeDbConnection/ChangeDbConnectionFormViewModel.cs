using System.Windows;
using System.Windows.Input;
using Calabonga.Results;
using Npgsql;
using PrintHub.WPF.Shared.Commands;
using PrintHub.WPF.Shared.Services;
using PrintHub.WPF.Shared.ViewModels;

namespace PrintHub.WPF.Endpoints.AdminEndpoints.ChangeDbConnection;

public class ChangeDbConnectionFormViewModel : ViewModelBase
{
    private readonly IDbConnectionService _dbConnectionService;
    private ICommand? _createConnectionStringCommand;
    private ICommand? _resetCommand;
    private ICommand? _restartCommand;

    private string? _connectionString;
    private string? _database;
    private string? _host;
    private string? _password;
    private string? _username;

    public ChangeDbConnectionFormViewModel(IDbConnectionService dbConnectionService)
    {
        _dbConnectionService = dbConnectionService;
        FillConnectionData(_dbConnectionService.GetConnectionString());
    }

    public string? Host
    {
        get => _host;
        set => Set(ref _host, value);
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
        _dbConnectionService.SetConnectionString(string.Empty);
        ConnectionString = _dbConnectionService.GetConnectionString();
        FillConnectionData(ConnectionString);
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

        Operation<bool, string> result = _dbConnectionService.IsDatabaseValid(builder.ConnectionString);

        if (result.Ok)
        {
            ConnectionString = builder.ConnectionString;
            _dbConnectionService.SetConnectionString(ConnectionString);

            MessageBoxResult boxResult = MessageBox.Show("A reboot is required to apply the changes!", "Attention", MessageBoxButton.OKCancel);

            if (boxResult == MessageBoxResult.OK)
                RestartCommand.Execute(null);

            return;
        }

        MessageBox.Show(result.Error, "Error");
    });

    private void FillConnectionData(string connectionString)
    {
        NpgsqlConnectionStringBuilder builder = new(connectionString);
        Host = builder.Host;
        Username = builder.Username;
        Password = builder.Password;
        Database = builder.Database;
    }
}