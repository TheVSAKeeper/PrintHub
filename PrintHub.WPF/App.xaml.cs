using System.Diagnostics;
using System.IO;
using System.Windows;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Npgsql;
using PrintHub.Infrastructure;
using PrintHub.WPF.Definitions.Base;
using PrintHub.WPF.Properties;

namespace PrintHub.WPF;

public partial class App
{
    private static IHost? _host;

    private static IHost Host => _host ??= Program.CreateHostBuilder(Environment.GetCommandLineArgs()).Build();

    private static IServiceProvider Services => Host.Services;

    protected override async void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        await Host.StartAsync();
        await Host.UseDefinitions();

        MainWindow = Services.GetRequiredService<MainWindow>();
        MainWindow.Show();
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        using IHost host = Host;

        base.OnExit(e);
        await host.StopAsync();
    }

    public static void RestartApplication()
    {
        Process.Start(Process.GetCurrentProcess().MainModule?.FileName ?? throw new InvalidOperationException());
        Current.Shutdown();
    }

    public static string GetConnectionString()
    {
        //MessageBox.Show(Settings.Default.ConnectionString);
        if (string.IsNullOrEmpty(Settings.Default.ConnectionString) == false)
            return Settings.Default.ConnectionString;

        IConfigurationBuilder configBuilder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json");

        IConfigurationRoot configuration = configBuilder.Build();
        return configuration.GetConnectionString(nameof(ApplicationDbContext)) ?? string.Empty;
    }

    public static void SetConnectionString(string connectionString)
    {
        if (IsConnectionStringValid(connectionString) == false)
            return;

        Settings.Default.ConnectionString = connectionString;
        Settings.Default.Save();
    }

    public static bool IsConnectionStringValid(string connectionString)
    {
        try
        {
            NpgsqlConnectionStringBuilder dummy = new(connectionString);
            return true;
        }
        catch
        {
            return false;
        }
    }
}