using Microsoft.Extensions.Hosting;
using PrintHub.WPF.Definitions.Base;
using Serilog;
using Serilog.Events;

namespace PrintHub.WPF;

internal class Program
{
    [STAThread]
    public static void Main(string[] args)
    {
        try
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();

            App app = new();

            app.InitializeComponent();
            app.Run();
        }
        catch (Exception exception)
        {
            string type = exception.GetType().Name;

            if (type.Equals("StopTheHostException", StringComparison.Ordinal))
                throw;

            Log.Fatal(exception, "Unhandled exception");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }

    public static IHostBuilder CreateHostBuilder(string[] args) => Host
        .CreateDefaultBuilder(args)
        .UseSerilog()
        .AddDefinitions(typeof(Program));
}