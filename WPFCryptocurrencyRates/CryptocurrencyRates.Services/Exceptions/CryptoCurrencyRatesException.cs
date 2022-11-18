namespace CryptocurrencyRates.Services.Exceptions;

public class CryptoCurrencyRatesException : Exception
{
    private readonly string errorCode;
    private readonly string errorMessage;

    public CryptoCurrencyRatesException(string message) : base(message)
    {
    }

    public CryptoCurrencyRatesException(string message, string errorCode, string errorMessage) : base(message)
    {
        this.errorCode = errorCode;
        this.errorMessage = errorMessage;
    }
}