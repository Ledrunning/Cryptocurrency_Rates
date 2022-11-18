namespace CryptocurrencyRates.Gateway.Configuration;

public class ConcapApi
{
    public const string SectionName = "ConcapApi";
    public string? BaseUrl { get; set; }
    public int Timeout { get; set; }
}