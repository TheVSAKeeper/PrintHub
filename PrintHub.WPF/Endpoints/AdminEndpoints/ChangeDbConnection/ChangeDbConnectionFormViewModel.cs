using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Calabonga.Results;
using FluentValidation;
using Npgsql;
using PrintHub.WPF.Shared.Commands;
using PrintHub.WPF.Shared.MaterialMessageBox;
using PrintHub.WPF.Shared.Services;
using PrintHub.WPF.Shared.ViewModels;

namespace PrintHub.WPF.Endpoints.AdminEndpoints.ChangeDbConnection;

public class ChangeDbConnectionFormViewModel : ValidationViewModel<ChangeDbConnectionFormViewModel>
{
    private readonly IDbConnectionService _dbConnectionService;

    private ICommand? _createConnectionStringCommand;
    private ICommand? _resetCommand;
    private ICommand? _restartCommand;

    private string? _connectionString;
    private string? _database;
    private string? _host;
    private string? _username;

    public ChangeDbConnectionFormViewModel(IDbConnectionService dbConnectionService, IValidator<ChangeDbConnectionFormViewModel> validator)
    {
        _dbConnectionService = dbConnectionService;
        Validator = validator;
        FillConnectionData(_dbConnectionService.GetConnectionString());
    }

    protected override ChangeDbConnectionFormViewModel ViewModel => this;
    protected sealed override IValidator<ChangeDbConnectionFormViewModel> Validator { get; init; }

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
        MessageBoxResult boxResult = MaterialMessageBox.ShowWithCancel("Are you sure you restart application?", "Restart");

        if (boxResult == MessageBoxResult.OK)
            App.RestartApplication();
    });

    public ICommand ResetCommand => _resetCommand ??= new LambdaCommand(() =>
    {
        _dbConnectionService.SetConnectionString(string.Empty);
        ConnectionString = _dbConnectionService.GetConnectionString();
        FillConnectionData(ConnectionString);
    });

    public ICommand CreateConnectionStringCommand => _createConnectionStringCommand ??= new LambdaCommand(parameter =>
    {
        if (parameter is not PasswordBox passwordBox)
            return;

        Validate();

        if (HasErrors)
            return;

        NpgsqlConnectionStringBuilder builder = new()
        {
            Host = Host,
            Username = Username,
            Password = passwordBox.Password,
            Database = Database
        };

        Operation<bool, string> result = _dbConnectionService.IsDatabaseValid(builder.ConnectionString);

        if (result.Ok)
        {
            ConnectionString = builder.ConnectionString;
            _dbConnectionService.SetConnectionString(ConnectionString);

            MessageBoxResult boxResult = MaterialMessageBox.ShowWarningWithCancel("A reboot is required to apply the changes!");

            if (boxResult == MessageBoxResult.OK)
                RestartCommand.Execute(null);

            return;
        }

        MaterialMessageBox.ShowError(result.Error);
    });

    private void FillConnectionData(string connectionString)
    {
        NpgsqlConnectionStringBuilder builder = new(connectionString);
        Host = builder.Host;
        Username = builder.Username;
        Database = builder.Database;
        
        Validate();
    }
}