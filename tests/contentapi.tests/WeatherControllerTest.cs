using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;

namespace contentapi.tests;

/// <summary>
/// Integration tests based on https://learn.microsoft.com/en-us/dotnet/architecture/microservices/multi-container-microservice-net-applications/test-aspnet-core-services-web-apps
/// TestServer in the Microsoft.AspNetCore.TestHost package is used to spin up a test server based on the application StartUp class.
/// </summary>
public class WeatherControllerTest
{
    private readonly HttpClient _httpClient;

    public WeatherControllerTest()
    {
        var testServer = new TestServer(new WebHostBuilder().UseStartup<Startup>());
        _httpClient = testServer.CreateClient();
    }

    [Fact]
    public async Task GetsWeather()
    {
        var response = await _httpClient.GetAsync("/weather");
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();
        dynamic weatherList = JsonConvert.DeserializeObject(responseString) ??
                              throw new InvalidOperationException("Response cannot be null");
        Assert.Equal(5, weatherList.Count);
    }
}