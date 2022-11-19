using System.Threading;
using System.Threading.Tasks;
using CryptoCurrencyRates.Client.Model;

namespace CryptoCurrencyRates.Client.Contracts;

public interface ICryptoCurrencyRatesService
{
    /// <summary>
    ///     Gets crypto rates from CryptocurrencyRates.Gateway for current task from iTero
    /// </summary>
    /// <param name="token"></param>
    /// <returns>RequiredRatesModel</returns>
    Task<RequiredRatesModel> GetCurrentRatesAsync(CancellationToken token);
}