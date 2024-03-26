using marketswebapp.Models;
using Newtonsoft.Json;

namespace marketswebapp;

public class ContentApi
{
    private readonly HttpClient _client;

    public ContentApi(HttpClient client)
    {
        _client = client;
        _client.BaseAddress = new Uri(Environment.GetEnvironmentVariable("CONTENT_API_URI") ?? "https://localhost:5001");
    }

    public async Task<IEnumerable<WeatherForecast>?> GetWeather()
    {
        var response = await _client.GetAsync("/weather");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<IEnumerable<WeatherForecast>>(content);
    }
}