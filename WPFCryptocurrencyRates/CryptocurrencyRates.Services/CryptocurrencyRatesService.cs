﻿using CryptocurrencyRates.Common.Exceptions;
using CryptocurrencyRates.Services.Constant;
using CryptocurrencyRates.Services.Contracts;
using CryptocurrencyRates.Services.Dto;
using Newtonsoft.Json;
using NLog;
using RestSharp;

namespace CryptocurrencyRates.Services;

/// <summary>
///     Class for getting currency rates from coincap.io
/// </summary>
public sealed class CryptoCurrencyRatesService : ICryptoCurrencyRatesService
{
    private readonly string baseUrl;
    private readonly int timeout;
    private readonly ILogger logger;

    public CryptoCurrencyRatesService(ILogger logger, string baseUrl, int timeout)
    {
        this.logger = logger;
        this.baseUrl = baseUrl;
        this.timeout = timeout;
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
        var url = new Uri($"{baseUrl}/{id}");
        var client = new RestClient(SetOptions(url));

        var request = new RestRequest();
        var responce = await client.ExecuteAsync(request, token);

        return GetContent<CurrencyRates>(responce, url.AbsoluteUri);
    }

    /// <inheritdoc cref="ICryptoCurrencyRatesService" />
    public async Task<List<Data>?> GetAllCryptoCurrencyRatesAsync(CancellationToken token)
    {
        var url = new Uri($"{baseUrl}");
        var client = new RestClient(SetOptions(url));

        var request = new RestRequest();
        var responce = await client.ExecuteAsync(request, token);

        var listOfRates = GetContent<AllCurrencyRates>(responce, url.AbsoluteUri);
        return listOfRates.Data;
    }

    private T GetContent<T>(RestResponse responce, string url)
    {
        if (responce.IsSuccessful)
        {
            if (responce.Content != null)
            {
                var model = JsonConvert.DeserializeObject<T>(responce.Content);
                logger.Info("Request for Coincap successfully finished {Url}", url);
                if (model != null)
                {
                    return model;
                }

                logger.Info("Requested data from Coincap is null {Url}", url);
            }
        }

        throw new CryptoCurrencyRatesException(
            $"Response from Concap failed. Status code: {responce.StatusCode}, {responce.ErrorMessage}");
    }

    private RestClientOptions SetOptions(Uri url)
    {
        return new RestClientOptions(url)
        {
            ThrowOnAnyError = true,
            MaxTimeout = timeout
        };
    }
}