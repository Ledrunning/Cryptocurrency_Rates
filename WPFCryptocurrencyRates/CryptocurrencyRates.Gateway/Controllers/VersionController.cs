using CryptocurrencyRates.Gateway.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace CryptocurrencyRates.Gateway.Controllers;

[Route("api/[controller]")]
[ApiController]
public class VersionController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return Ok($"Version - {ProgramRuntime.ProgramVersion}");
    }
}