namespace SprinklerAgent.App.LogicSteps
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Configuration;
    using DeadSimpleCache;
    using Logging;
    using LogicLadder;
    using Microsoft.ApplicationInsights.DataContracts;
    using Models;
    using Newtonsoft.Json;
    using Weather;

    public class GetWeatherData : IStep<SprinklerAgentContext>
    {
        private readonly IWeatherApiClient _weatherApi;
        private readonly IConfig _config;
        private readonly SimpleCache _cache;
        private readonly IAppLogger _logger;

        public GetWeatherData(
            IWeatherApiClient weatherApi,
            IConfig config,
            SimpleCache cache,
            IAppLogger logger)
        {
            _weatherApi = weatherApi;
            _config = config;
            _cache = cache;
            _logger = logger;
        }

        public bool ContinueOnError => true;
        public async Task<SprinklerAgentContext> RunStep(SprinklerAgentContext context)
        {
            var cached = _cache.Get<WeatherApiCacheInfo>(CacheKeys.WeatherApiResult);
            if (cached.IsCacheNull == false
                && cached.ValueOrDefault.CachedAtDateTime <= DateTime.Now.AddMinutes(10))
            {
                context.WeatherData = cached.ValueOrDefault;
            }
            else
            {
                var weatherResponse = await _weatherApi.GetWeatherByUSZip(_config.WeatherApiZip());
                if (weatherResponse.IsSuccess)
                {
                    context.WeatherData = new WeatherApiCacheInfo()
                    {
                        CachedAtDateTime = DateTime.Now,
                        WeatherApiResult = weatherResponse
                    };
                    _cache.Set(CacheKeys.WeatherApiResult, context.WeatherData);
                }
            }

            context.IsWeatherApiRainDelay = IsWeatherApiRainDelay(context.WeatherData);

            return context;
        }

        public bool IsWeatherApiRainDelay(WeatherApiCacheInfo weatherData)
        {
            //https://openweathermap.org/weather-conditions  ?? not tested 1-27-2020, waiting for rain, lol.
            var result = false;

            int? id = weatherData?.WeatherApiResult?.Payload?.weather.FirstOrDefault()?.id;
            if (id.HasValue
                && ((id.Value >= 200//thunderstorms
                     && id.Value <= 232)
                    || (id.Value >= 300//drizzle
                        && id.Value <= 321)
                    || (id.Value >= 500//rain
                        && id.Value <= 531)
                    || (id.Value >= 600//snow
                        && id.Value <= 622)))
            {
                result = true;
                _logger.Log(message: $"{this.GetType().Name}_IsWeatherApiRainDelay Found Rain",
                    level: SeverityLevel.Warning,
                    properties: new Dictionary<string, string>
                    {
                        {LogLabels.RainWeatherApi, JsonConvert.SerializeObject(weatherData)}
                    });
            }

            return result;
        }
    }
}
