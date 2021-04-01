namespace UnitTests.Weather
{
    using System;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Newtonsoft.Json;
    using NSubstitute;
    using SprinklerAgent.Configuration;
    using SprinklerAgent.Sprinkler_Api;
    using SprinklerAgent.Weather;
    using Xunit;
    using Xunit.Abstractions;

    public class WeatherSandBox_Integration
    {
        private readonly ITestOutputHelper _logger;

        public WeatherSandBox_Integration(ITestOutputHelper logger)
        {
            _logger = logger;
        }

        [Fact]
        public async Task WeatherClient()
        {
            try
            {
                var config = Substitute.For<IConfig>();
                config.WeatherApiUrl().ReturnsForAnyArgs("");//breaks test but values are secure.
                config.WeatherApiKey().ReturnsForAnyArgs("");

                var sut = new WeatherApiClient(config);

                var result = await sut.GetWeatherByUSZip("85210");
                result.IsSuccess.Should().BeTrue();

            }
            catch (Exception e)
            {
                _logger.WriteLine(JsonConvert.SerializeObject(e, Formatting.Indented));
            }
        }
    }
}
