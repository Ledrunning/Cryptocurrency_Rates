namespace CryptocurrencyRates.Services.Dto;

public class CurrencyRateModel
{
    public RateModel? Rate { get; set; }
    public long Timestamp { get; set; }
    public List<RateModel>? AllRates { get; set; }

    public class RateModel
    {
        public string? Id { get; set; }
        public string? Symbol { get; set; }
        public string? CurrencySymbol { get; set; }
        public string? Type { get; set; }
        public decimal? RateUsd { get; set; }
    }
}