using System.Diagnostics;
using CryptocurrencyRates.Gateway.Extensions;
using NLog;
using NLog.Web;

namespace CryptocurrencyRates.Gateway;

internal static class Program
{
    public static void Main(string[] args)
    {
        var loggerConfig = "NLog.config";
        var logger = NLogBuilder.ConfigureNLog(loggerConfig).GetCurrentClassLogger();

        try
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.ConfigureLogging();
            
            var app = builder.Build();
            app.Lifetime.RegisterApplicationLifetimeDelegates(logger);

            app.Run();
        }
        catch (Exception ex)
        {
            var name = typeof(Program).Assembly.GetName().Name;
            Trace.Write($"[{DateTime.Now:HH:mm:ss.fff}] Ошибка при старте приложения [{name}]! Подробности {ex.Message}");
            logger.Fatal(ex, $"Произошла ошибка при старте приложения [{name}]");
        }
        finally
        {
            LogManager.Shutdown();
        }
    }
}