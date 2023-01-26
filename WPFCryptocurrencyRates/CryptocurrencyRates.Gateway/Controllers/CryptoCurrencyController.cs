using CryptocurrencyRates.Services.Contracts;
using CryptocurrencyRates.Services.Dto;
using Microsoft.AspNetCore.Mvc;

namespace CryptocurrencyRates.Gateway.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CryptoCurrencyController : Controller
{
    private readonly ICryptoCurrencyRatesService cryptoCurrencyService;

    public CryptoCurrencyController(ICryptoCurrencyRatesService cryptoCurrencyService)
    {
        this.cryptoCurrencyService = cryptoCurrencyService;
    }

    [HttpGet]
    [Route(nameof(GetCryptoCurrencyRateById))]
    public async Task<CurrencyRates> GetCryptoCurrencyRateById(string id, CancellationToken token)
    {
        return await cryptoCurrencyService.GetCryptoCurrencyRateByIdAsync(id, token);
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

    [HttpGet]
    [Route(nameof(GetCurrentRatesList))]
    public Task<List<CurrencyRates>> GetCurrentRatesList(CancellationToken token)
    {
        return cryptoCurrencyService.GetCurrentRatesListAsync(token);
    }
}