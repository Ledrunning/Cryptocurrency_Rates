using CryptocurrencyRates.Services.Contracts;
using CryptocurrencyRates.Services.Dto;
using Microsoft.AspNetCore.Mvc;

namespace CryptocurrencyRates.Gateway.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CryptoCurrencyController : Controller
{
    private readonly ICryptocurrencyRatesService cryptoCurrencyService;

    public CryptoCurrencyController(ICryptocurrencyRatesService cryptoCurrencyService)
    {
        this.cryptoCurrencyService = cryptoCurrencyService;
    }

    [HttpGet]
    [Route(nameof(GetCryptoCurrencyRateById))]
    public Task<CurrencyRates> GetCryptoCurrencyRateById(string id, CancellationToken token)
    {
        return cryptoCurrencyService.GetCryptoCurrencyRateByIdAsync(id, token);
    }

    [HttpGet]
    [Route(nameof(GetCurrentRates))]
    public async Task<RequiredRatesModel> GetCurrentRates(CancellationToken token)
    {
        return await cryptoCurrencyService.GetCurrentRatesAsync(token);
    }

    [HttpGet]
    [Route(nameof(GetAllCryptoCurrencyRates))]
    public Task<List<Data>?> GetAllCryptoCurrencyRates(CancellationToken token)
    {
        return cryptoCurrencyService.GetAllCryptoCurrencyRatesAsync(token);
    }
}