﻿using CryptocurrencyRates.Services.Contracts;
using CryptocurrencyRates.Services.Dto;
using CryptocurrencyRates.Services.Exceptions;
using Newtonsoft.Json;
using NLog;
using RestSharp;
using static CryptocurrencyRates.Services.Dto.CurrencyRateModel;

namespace CryptocurrencyRates.Services;

/// <summary>
///     Class for getting currency rates from coincap.io
/// </summary>
public sealed class CryptocurrencyRatesService : ICryptocurrencyRatesService
{
    private readonly string baseUrl;
    private readonly ILogger logger;

    public CryptocurrencyRatesService(ILogger logger, string baseUrl)
    {
        this.logger = logger;
        this.baseUrl = baseUrl;
    }

    /// <inheritdoc cref="ICryptocurrencyRatesService" />
    public async Task<RequiredRatesModel> GetCurrentRates(CancellationToken token)
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

    /// <inheritdoc cref="ICryptocurrencyRatesService" />
    public async Task<CurrencyRateModel> GetCryptoCurrencyRateByIdAsync(string id, CancellationToken token)
    {
        var url = new Uri($"{baseUrl}/{id}");
        var client = new RestClient(SetOptions(url));

        var request = new RestRequest();
        var responce = await client.ExecuteAsync(request, token);

        return GetContent<CurrencyRateModel>(responce, url.AbsoluteUri);
    }

    /// <inheritdoc cref="ICryptocurrencyRatesService" />
    public async Task<List<RateModel>?> GetAllCryptoCurrencyRates(CancellationToken token)
    {
        var url = new Uri($"{baseUrl}");
        var client = new RestClient(SetOptions(url));

        var request = new RestRequest();
        var responce = await client.ExecuteAsync(request, token);

        var listOfRates = GetContent<CurrencyRateModel>(responce, url.AbsoluteUri).AllRates;
        return listOfRates;
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
            $"Response from OcrolusGateway failed. Status code: {responce.StatusCode}, {responce.ErrorMessage}");
    }

    private RestClientOptions SetOptions(Uri url)
    {
        return new RestClientOptions(url)
        {
            ThrowOnAnyError = true,
            MaxTimeout = -1
        };
    }
}