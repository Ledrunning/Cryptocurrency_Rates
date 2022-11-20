using System.Threading;
using System.Threading.Tasks;
using CryptoCurrencyRates.Client.Contracts;
using CryptoCurrencyRates.Client.Model;
using CryptocurrencyRates.Common.Exceptions;
using Newtonsoft.Json;
using RestSharp;

namespace CryptoCurrencyRates.Client.Services.Rest;

public class CryptoCurrencyRatesService : ICryptoCurrencyRatesService
{
    private readonly string url;

    public CryptoCurrencyRatesService(string url)
    {
        this.url = url;
    }

    /// <inheritdoc cref="ICryptoCurrencyRatesService" />
    public async Task<RequiredRatesModel> GetCurrentRatesAsync(CancellationToken token)
    {
        var client = new RestClient(url);

        var request = new RestRequest();
        var responce = await client.ExecuteAsync(request, token);

        return GetContent(responce);
    }

    private RequiredRatesModel GetContent(RestResponse responce)
    {
        if (responce.IsSuccessful)
        {
            if (responce.Content != null)
            {
                var model = JsonConvert.DeserializeObject<RequiredRatesModel>(responce.Content);
                if (model != null)
                {
                    return model;
                }
            }
        }

        throw new CryptoCurrencyRatesException(
            $"Response from CryptocurrencyRates.Gateway is failed. Status code: {responce.StatusCode}, {responce.ErrorMessage}");
    }
}