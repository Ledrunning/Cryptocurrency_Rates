using System;
using System.IO;
using System.Text;
using System.Windows;
using CryptoCurrencyRates.Client.Configuration;
using CryptoCurrencyRates.Client.Contracts;
using CryptoCurrencyRates.Client.Services.Rest;
using CryptoCurrencyRates.Client.View;
using CryptoCurrencyRates.Client.ViewModel;
using CryptocurrencyRates.Common.Exceptions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CryptoCurrencyRates.Client;

/// <summary>
///     Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    public IServiceProvider? ServiceProvider { get; private set; }
    public IConfiguration? Configuration { get; private set; }

    protected override void OnStartup(StartupEventArgs eventArgs)
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", false, true);

        Configuration = builder.Build();

        var serviceCollection = new ServiceCollection();
        ConfigureServices(serviceCollection);
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        ServiceProvider = serviceCollection.BuildServiceProvider();
        var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
        mainWindow.Show();
    }

    private void ConfigureServices(IServiceCollection services)
    {
        var gatewaySettings = Configuration?.GetSection(GatewaySettings.SectionName).Get<GatewaySettings>();

        services.AddTransient<ICryptoCurrencyRatesService>(x =>
        {
            if (gatewaySettings is { GatewayUrl: { } })
            {
                return new CryptoCurrencyRatesService(gatewaySettings.GatewayUrl);
            }

            throw new CryptoCurrencyRatesException("Error to reading gateway settings!");
        });
        // Register all ViewModels.
        services.AddSingleton<MainViewModel>();

        // Register all the Windows of the applications.
        services.AddScoped<MainWindow>(_ => new() {DataContext = _.GetService<MainViewModel>()});
    }
}