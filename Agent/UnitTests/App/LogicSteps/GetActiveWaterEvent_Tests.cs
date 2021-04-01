namespace UnitTests.App.LogicSteps
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using DeadSimpleCache;
    using FluentAssertions;
    using Microsoft.ApplicationInsights.DataContracts;
    using NSubstitute;
    using SprinklerAgent.App;
    using SprinklerAgent.App.LogicSteps;
    using SprinklerAgent.App.Models;
    using SprinklerAgent.Configuration;
    using SprinklerAgent.Logging;
    using SprinklerAgent.Sprinkler_Api;
    using SprinklerAgent.Sprinkler_Api.Models;
    using SprinklerAgent.Weather;
    using SprinklerAgent.Weather.Models;
    using Xunit;

    public class GetActiveWaterEvent_Tests
    {
        [Theory]
        [InlineData(-1, 1, false)]
        [InlineData(0, 1, false)]
        [InlineData(1, 2, true)]
        [InlineData(5, 10, true)]
        [InlineData(-5, -1, true)]
        public async Task Run(int relativeStartSeconds, int relativeEndSeconds, bool isEventNull)
        {
            var cache = new SimpleCache();
            var sprinklerManager = Substitute.For<ISprinklerManager>();
            var weatherApiClient = Substitute.For<IWeatherApiClient>();
            weatherApiClient.GetWeatherByUSZip(Arg.Any<string>())
                .ReturnsForAnyArgs(Task.FromResult(new HttpResponse<WeatherApiResultModel>()
                {
                    IsSuccess = true,
                    Payload = new WeatherApiResultModel()
                    {
                        weather = new List<Weather>()
                        {
                            new Weather(){id = 800}
                        }
                    }
                }));
            var config = Substitute.For<IConfig>();
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
            var ctx = new SprinklerAgentContext
            {
                Schedule = new ScheduleModel()
                {
                    WaterEvents = new List<WeeklyWaterEvent>
                    {
                        new WeeklyWaterEvent
                        {
                            ZoneName = ZoneLabels.NorthGrass,
                            DayOfWeek = DateTime.Now.DayOfWeek,
                            Start = DateTime.Now.TimeOfDay.Add(TimeSpan.FromSeconds(relativeStartSeconds)),
                            End = DateTime.Now.TimeOfDay.Add(TimeSpan.FromSeconds(relativeEndSeconds))
                        }
                    }
                }
            };

            var sut = new GetActiveWaterEvent(
                logger: logger,
                sprinklerManager: sprinklerManager,
                weatherApiClient: weatherApiClient,
                config: config,
                cache: cache);
            var result = await sut.RunStep(ctx);
            (result.ActiveWaterEvent == null).Should().Be(isEventNull);
        }

        [Theory]
        [InlineData(-5, false)]
        [InlineData(5, true)]
        [InlineData(null, false)]
        public async Task IsManualRainDelay(int? relativeStartSeconds, bool isRainDelay)
        {
            var cache = new SimpleCache();
            var sprinklerApi = Substitute.For<ISprinklerApiClient>();
            RainDelay rainDelay = null;
            if (relativeStartSeconds.HasValue)
            {
                rainDelay = new RainDelay()
                {
                    RainDelayUid = Guid.NewGuid(),
                    RainDelayExpireDate = DateTime.Now.AddSeconds(relativeStartSeconds.Value)
                };
            }
            sprinklerApi.GetSchedule().ReturnsForAnyArgs(Task.FromResult(new StandardResponse<ScheduleModel>()
            {
                StatusCode = 200,
                ValidationMessages = new List<string>(),
                Payload = new ScheduleModel
                {
                    RainDelay = rainDelay,
                    WaterEvents = new List<WeeklyWaterEvent>
                    {
                        new WeeklyWaterEvent()
                    }
                }
            }));

            var sut = new GetSchedule(sprinklerApi: sprinklerApi, cache: cache);

            var result = await sut.RunStep(new SprinklerAgentContext());

            result.IsManualRainDelay.Should().Be(isRainDelay);
        }

        [Theory]
        [InlineData(800, false)]
        [InlineData(500, true)]
        [InlineData(701, false)]
        [InlineData(600, true)]
        public async Task IsWeatherApiRainDelay(int apiWeatherResponseId, bool isRainDelay)
        {
            //https://openweathermap.org/weather-conditions

            var cache = new SimpleCache();
            var weatherApiClient = Substitute.For<IWeatherApiClient>();
            weatherApiClient.GetWeatherByUSZip(Arg.Any<string>())
                .ReturnsForAnyArgs(Task.FromResult(new HttpResponse<WeatherApiResultModel>()
                {
                    IsSuccess = true,
                    Payload = new WeatherApiResultModel()
                    {
                        weather = new List<Weather>()
                        {
                            new Weather(){id = apiWeatherResponseId}
                        }
                    }
                }));
            var config = Substitute.For<IConfig>();
            config.WeatherApiZip().ReturnsForAnyArgs("85210");
            var logger = Substitute.For<IAppLogger>();
            var sut = new GetWeatherData(weatherApi: weatherApiClient, config: config, cache: cache, logger: logger);

            var result = await sut.RunStep(new SprinklerAgentContext());

            result.IsWeatherApiRainDelay.Should().Be(isRainDelay);
            cache.Get<WeatherApiCacheInfo>(CacheKeys.WeatherApiResult).IsCacheNull.Should().BeFalse();
            cache.Get<WeatherApiCacheInfo>(CacheKeys.WeatherApiResult).ValueOrDefault?.WeatherApiResult?.Payload?.weather[0].id.Should().Be(apiWeatherResponseId);
        }
    }
}
