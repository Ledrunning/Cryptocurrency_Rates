using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CryptoCurrencyRates.Client.Model;

namespace CryptoCurrencyRates.Client.Contracts;

public interface ICryptoCurrencyRatesService
{
    /// <summary>
    ///     Get all crypto rates from coincap
    /// </summary>
    /// <param name="token"></param>
    /// <returns>list of CurrencyRateModel?l</returns>
    Task<List<CurrencyRateModel>?> GetAllCryptoCurrencyRatesAsync(CancellationToken token);
}