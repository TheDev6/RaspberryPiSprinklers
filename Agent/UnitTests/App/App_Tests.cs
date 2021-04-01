namespace UnitTests.App
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Microsoft.ApplicationInsights.DataContracts;
    using Newtonsoft.Json;
    using NSubstitute;
    using SprinklerAgent.App;
    using SprinklerAgent.App.Models;
    using SprinklerAgent.AzureBlob;
    using SprinklerAgent.Configuration;
    using SprinklerAgent.Logging;
    using SprinklerAgent.Sprinkler_Api;
    using SprinklerAgent.Weather;
    using SprinklerAgent.Weather.Models;
    using Xunit;
    using Xunit.Abstractions;


    public class App_Tests
    {
        private readonly ITestOutputHelper _log;

        public App_Tests(ITestOutputHelper log)
        {
            _log = log;
        }

        [Fact]
        public void Run_HappyPath()
        {
            var config = Substitute.For<IConfig>();
            config.RunFrequencySeconds().ReturnsForAnyArgs(1);
            var sprinklerManager = Substitute.For<ISprinklerManager>();
            //var blobClient = Substitute.For<IAzureBlobClient>();
            var weatherApiClient = Substitute.For<IWeatherApiClient>();
            //var sprinklerApi = Substitute.For<ISprinklerApiClient>();

            var logger = Substitute.For<IAppLogger>();
            var logs = new List<dynamic>();
            logger.When(x => x.Log(
                    message: Arg.Any<string>(),
                    level: Arg.Any<SeverityLevel>(),
                    properties: Arg.Any<Dictionary<string, string>>()))
                .Do(ctx =>
                {
                    dynamic obj = new
                    {
                        Message = ctx[0],
                        Level = ctx[1],
                        Properties = ctx[2]
                    };
                    logs.Add(obj);
                });
            var runCount = 0;
            var stopCount = 1;
            var runUntil = new Func<bool>(() =>
            {
                runCount++;
                return runCount <= stopCount;
            });

            var sut = AppBuilder.Build(
                inConfig: config,
                inAppLogger: logger,
                inSprinklerManager: sprinklerManager,
                runUntil: runUntil,
                inWeatherApiClient: weatherApiClient
            );

            sut.Run();

            logs.Count.Should().BeGreaterThan(0);
            _log.WriteLine(JsonConvert.SerializeObject(logs));
        }
    }
}
