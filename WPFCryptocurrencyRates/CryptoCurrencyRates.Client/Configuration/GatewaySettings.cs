namespace CryptoCurrencyRates.Client.Configuration;

public class GatewaySettings
{
    public const string SectionName = "CryptocurrencyRatesGateway";
    public string? GatewayUrl { get; set; }
}
