namespace CryptocurrencyRates.Services.Dto;

public class RequiredRatesModel
{
    public CurrencyRateModel? Bitcoin { get; set; }
    public CurrencyRateModel? Dogecoin { get; set; }
    public CurrencyRateModel? Ethereum { get; set; }
}