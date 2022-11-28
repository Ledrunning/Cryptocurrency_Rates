using CryptocurrencyRates.Common.Exceptions;
using Newtonsoft.Json;
using NLog;
using RestSharp;

namespace CryptocurrencyRates.Services.Services;

public class BaseService
{
    protected readonly string BaseUrl;
    private readonly ILogger logger;
    private readonly int timeout;

    public BaseService(ILogger logger, string baseUrl, int timeout)
    {
        this.logger = logger;
        BaseUrl = baseUrl;
        this.timeout = timeout;
    }

    protected T GetContent<T>(RestResponseBase response, string url)
    {
        if (response.IsSuccessful)
        {
            if (response.Content != null)
            {
                var model = JsonConvert.DeserializeObject<T>(response.Content);
                logger.Info("Request for Coincap successfully finished {Url}", url);
                if (model != null)
                {
                    return model;
                }
                logger.Info("Requested data from Coincap is null {Url}", url);
            }
        }

        throw new CryptoCurrencyRatesException(
            $"Response from Concap failed. Status code: {response.StatusCode}, {response.ErrorMessage}");
    }

    protected RestClientOptions SetOptions(Uri url)
    {
        return new RestClientOptions(url)
        {
            ThrowOnAnyError = true,
            MaxTimeout = timeout
        };
    }
}