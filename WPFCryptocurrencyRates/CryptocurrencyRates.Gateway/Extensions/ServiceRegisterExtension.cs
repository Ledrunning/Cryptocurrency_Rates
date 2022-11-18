using CryptocurrencyRates.Gateway.Helpers;
using NLog;

namespace CryptocurrencyRates.Gateway.Extensions;

static class ServiceRegisterExtension
{
    public static void RegisterApplicationLifetimeDelegates(this IHostApplicationLifetime hostApplicationLifetime,
        Logger logger)
    {
        hostApplicationLifetime.ApplicationStarted.Register(() =>
        {
            logger.Info(
                $"CryptocurrencyRates.Gateway has been started : [{ProgramRuntime.ProgramName}] ({ProgramRuntime.ProgramVersion}) Rev({ProgramRuntime.ProgramRevision})");
            logger.Info($"Program Version - {ProgramRuntime.ProgramVersion}");
        });

        hostApplicationLifetime.ApplicationStopped.Register(() =>
        {
            logger.Info(
                $"CryptocurrencyRates.Gateway has been stopped: [{ProgramRuntime.ProgramName}] ({ProgramRuntime.ProgramVersion}) Rev({ProgramRuntime.ProgramRevision})");
            logger.Info($"Program Version - {ProgramRuntime.ProgramVersion}");
        });
    }

    public static void RegisterSwagger(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", $"{ProgramRuntime.ProgramVersion}");
                c.RoutePrefix = string.Empty;
            });
        }
    }
}