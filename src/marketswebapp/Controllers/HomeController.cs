using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using marketswebapp.Models;

namespace marketswebapp.Controllers;

public class HomeController : Controller
{
    private readonly ContentApi _contentApi;
    private readonly ILogger<HomeController> _logger;

    public HomeController(ContentApi contentApi, ILogger<HomeController> logger)
    {
        _contentApi = contentApi;
        _logger = logger;
    }

    public async Task<IActionResult> Index()
    {
        IEnumerable<WeatherForecast>? forecast = await _contentApi.GetWeather();
        return View(forecast);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
