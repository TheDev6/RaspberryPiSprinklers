namespace UnitTests.SprinklerApi
{
    using System;
    using System.Threading.Tasks;
    using NSubstitute;
    using SprinklerAgent.Configuration;
    using SprinklerAgent.Sprinkler_Api;
    using SprinklerAgent.Sprinkler_Api.Models;
    using Xunit;

    public class SprinklerApi_Integration
    {
        [Fact]
        public async Task GetWaterEvents()
        {
            var config = Substitute.For<IConfig>();
            config.SprinklerApiUrl().ReturnsForAnyArgs("https://sprinklerapi.azurewebsites.net");
            config.SprinklerApiCredentials().ReturnsForAnyArgs("");//secure
            var sut = new SprinklerApiClient(config);

            var result = await sut.GetWaterEvents();

            var x = result;
        }

        [Fact]
        public async Task CreateRainDelay()
        {
            var config = Substitute.For<IConfig>();
            config.SprinklerApiUrl().ReturnsForAnyArgs("https://sprinklerapi.azurewebsites.net");
            config.SprinklerApiCredentials().ReturnsForAnyArgs("");//secure
            var sut = new SprinklerApiClient(config);

            var result = await sut.CreateRainDelay(new RainDelayCreateModel()
            { RainDelayExpireDateTime = DateTime.Now.AddDays(2) });

            var x = result;
        }
    }
}
