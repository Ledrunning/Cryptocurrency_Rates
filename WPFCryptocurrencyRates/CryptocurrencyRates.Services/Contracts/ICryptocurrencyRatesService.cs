using CryptocurrencyRates.Services.Dto;

namespace CryptocurrencyRates.Services.Contracts;

public interface ICryptocurrencyRatesService
{
    /// <summary>
    ///     Gets crypto rates for current task from iTero
    /// </summary>
    /// <param name="token"></param>
    /// <returns>RequiredRatesModel</returns>
    Task<RequiredRatesModel> GetCurrentRatesAsync(CancellationToken token);

    /// <summary>
    ///     Example request = "api.coincap.io/v2/rates/bitcoin"
    /// </summary>
    /// <param name="id"></param>
    /// <param name="token"></param>
    /// <returns>CurrencyRateModel</returns>
    Task<CurrencyRates> GetCryptoCurrencyRateByIdAsync(string id, CancellationToken token);

    /// <summary>
    ///     Get all crypto rates from coincap
    /// </summary>
    /// <param name="token"></param>
    /// <returns>list of RateMode?l</returns>
    Task<List<Data>?> GetAllCryptoCurrencyRatesAsync(CancellationToken token);
}