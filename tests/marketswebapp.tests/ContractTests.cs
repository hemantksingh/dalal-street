using Microsoft.AspNetCore.Mvc.Testing;
using PactNet;
using PactNet.Output.Xunit;
using Xunit.Abstractions;

namespace marketswebapp.tests;

public class ContractTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly ITestOutputHelper _output;
    private readonly WebApplicationFactory<Program> _factory;
    private readonly IPactBuilderV4? _pactBuilder;

    public ContractTests(ITestOutputHelper output, WebApplicationFactory<Program> factory)
    {
        var pactConfig = new PactConfig
        {
            PactDir = "../../../../pacts",
            Outputters = new[]
            {
                new XunitOutput(output)
            },
            LogLevel = PactLogLevel.Debug
        };

        _pactBuilder = Pact.V4("Markets app", "Markets Content API", pactConfig).WithHttpInteractions();
        _output = output;
        _factory = factory;
    }

    [Fact]
    public async void GetWeather_ReturnsExpectedResponse()
    {
        _pactBuilder?.UponReceiving("A GET request to the weather endpoint")
            .Given("Some Markets Provider API")
            .WithRequest(HttpMethod.Get, "/weather")
            .WillRespond()
            .WithStatus(System.Net.HttpStatusCode.OK)
            .WithJsonBody( new [] {
                new 
                {
                    Date = DateTime.Now,
                    TemperatureC = -10,
                    Summary = "Freezing"
                }});
        await _pactBuilder?.VerifyAsync(async ctx =>
        {
            _output.WriteLine($"Mock Content API URI: {ctx.MockServerUri}");
            Environment.SetEnvironmentVariable("CONTENT_API_URI", ctx.MockServerUri.ToString());

            var client = _factory.CreateClient();
            var response = await client.GetAsync("/home");
            response.EnsureSuccessStatusCode();
        })!;
    }
}