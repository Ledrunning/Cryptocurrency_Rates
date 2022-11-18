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
}