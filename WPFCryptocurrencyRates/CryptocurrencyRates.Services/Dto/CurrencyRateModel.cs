namespace CryptocurrencyRates.Services.Dto;

public class Data
{
    public string? Id { get; set; }
    public string? Symbol { get; set; }
    public string? CurrencySymbol { get; set; }
    public string? Type { get; set; }
    public string? RateUsd { get; set; }
}

public class CurrencyRates
{
    public Data? Data { get; set; }
    public long? Timestamp { get; set; }
}

public class AllCurrencyRates
{
    public List<Data>? Data { get; set; }
    public long Timestamp { get; set; }
}