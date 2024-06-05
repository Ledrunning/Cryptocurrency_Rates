using CryptocurrencyRates.Services.Constants;
using CryptocurrencyRates.Services.Contracts;
using CryptocurrencyRates.Services.Dto;
using NLog;
using RestSharp;

namespace CryptocurrencyRates.Services.Services;

/// <summary>
///     Class for getting currency rates from coincap.io
/// </summary>
public sealed class CryptoCurrencyRatesService : BaseService, ICryptoCurrencyRatesService
{
    public CryptoCurrencyRatesService(ILogger logger, string baseUrl, int timeout) : base(logger, baseUrl, timeout)
    {
    }

    /// <inheritdoc cref="ICryptoCurrencyRatesService" />
    public async Task<RequiredRatesModel> GetCurrentRatesAsync(CancellationToken token)
    {
        var bitcoin = await GetCryptoCurrencyRateByIdAsync(RatesIdConstants.Bitcoin, token);
        var dogecoin = await GetCryptoCurrencyRateByIdAsync(RatesIdConstants.Dogecoin, token);
        var ethereum = await GetCryptoCurrencyRateByIdAsync(RatesIdConstants.Ethereum, token);

        return new RequiredRatesModel
        {
            Bitcoin = bitcoin,
            Dogecoin = dogecoin,
            Ethereum = ethereum
        };
    }

    /// <inheritdoc cref="ICryptoCurrencyRatesService" />
    public async Task<List<CurrencyRates>> GetCurrentRatesListAsync(CancellationToken token)
    {
        var listOfCryptoCurrency = new List<CurrencyRates>();
        var bitcoin = await GetCryptoCurrencyRateByIdAsync(RatesIdConstants.Bitcoin, token);
        var dogecoin = await GetCryptoCurrencyRateByIdAsync(RatesIdConstants.Dogecoin, token);
        var ethereum = await GetCryptoCurrencyRateByIdAsync(RatesIdConstants.Ethereum, token);

        listOfCryptoCurrency.Add(bitcoin);
        listOfCryptoCurrency.Add(dogecoin);
        listOfCryptoCurrency.Add(ethereum);

        return listOfCryptoCurrency;
    }

    /// <inheritdoc cref="ICryptoCurrencyRatesService" />
    public async Task<CurrencyRates> GetCryptoCurrencyRateByIdAsync(string id, CancellationToken token)
    {
        var url = new Uri($"{BaseUrl}/{id}");
        var client = new RestClient(SetOptions(url));

        var request = new RestRequest();
        var response = await client.ExecuteAsync(request, token);

        return GetContent<CurrencyRates>(response, url.AbsoluteUri);
    }

    /// <inheritdoc cref="ICryptoCurrencyRatesService" />
    public async Task<List<Data>?> GetAllCryptoCurrencyRatesAsync(CancellationToken token)
    {
        var url = new Uri($"{BaseUrl}");
        var client = new RestClient(SetOptions(url));

        var request = new RestRequest();
        var response = await client.ExecuteAsync(request, token);

        var listOfRates = GetContent<AllCurrencyRates>(response, url.AbsoluteUri);
        return listOfRates.Data;
    }
}