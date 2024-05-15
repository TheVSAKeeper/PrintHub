using System.Diagnostics;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PrintHub.WPF.Definitions.Base;

namespace PrintHub.WPF;

public partial class App
{
    private static IHost? _host;

    private static IHost Host => _host ??= Program.CreateHostBuilder(Environment.GetCommandLineArgs()).Build();

    public static IServiceProvider Services => Host.Services;

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
}