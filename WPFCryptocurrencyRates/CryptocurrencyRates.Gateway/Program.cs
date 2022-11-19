using System.Diagnostics;
using CryptocurrencyRates.Gateway.Configuration;
using CryptocurrencyRates.Gateway.Extensions;
using CryptocurrencyRates.Gateway.Helpers;
using CryptocurrencyRates.Services;
using CryptocurrencyRates.Services.Contracts;
using Microsoft.OpenApi.Models;
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

            var concapConfig = builder.Configuration.GetSection(ConcapApi.SectionName).Get<ConcapApi>();

            //Configure services
            builder.Services.AddTransient<ICryptoCurrencyRatesService>(x =>
                new CryptoCurrencyRatesService(logger, concapConfig.BaseUrl!, concapConfig.Timeout));
            builder.Services.AddControllers();
            builder.Services.AddMvc();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1",
                    new OpenApiInfo
                    {
                        Title = "CryptocurrencyRates.Gateway", Version = $"{ProgramRuntime.ProgramFileVersion}"
                    });
            });

            var app = builder.Build();

            app.MapControllers();
            app.UseRouting();

            app.Lifetime.RegisterApplicationLifetimeDelegates(logger);

            app.RegisterSwagger();

            app.Run();
        }
        catch (Exception ex)
        {
            var name = typeof(Program).Assembly.GetName().Name;
            Trace.Write(
                $"[{DateTime.Now:HH:mm:ss.fff}] Application startup error [{name}]! Details {ex.Message}");
            logger.Fatal(ex, $"Application startup error [{name}]");
        }
        finally
        {
            LogManager.Shutdown();
        }
    }
}