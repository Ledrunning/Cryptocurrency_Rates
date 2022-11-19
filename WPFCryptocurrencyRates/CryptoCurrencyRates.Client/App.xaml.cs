using System;
using System.IO;
using System.Text;
using System.Windows;
using CryptoCurrencyRates.Client.Configuration;
using CryptoCurrencyRates.Client.Contracts;
using CryptoCurrencyRates.Client.Services.Rest;
using CryptoCurrencyRates.Client.View;
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
            new CryptoCurrencyRatesService(gatewaySettings?.GatewayUrl!));
        services.AddTransient(typeof(MainWindow));
    }
}