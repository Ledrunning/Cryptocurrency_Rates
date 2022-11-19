namespace CryptoCurrencyRates.Client.Model;
public class CurrencyRateModel
{
    public string? Id { get; set; }
    public string? Symbol { get; set; }
    public string? CurrencySymbol { get; set; }
    public string? Type { get; set; }
    public string? RateUsd { get; set; }
}

public class CurrencyRates
{
    public CurrencyRateModel? Data { get; set; }
    public long? Timestamp { get; set; }
}