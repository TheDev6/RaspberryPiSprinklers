namespace SprinklerAgent.App.Models
{
    using System;
    using Weather.Models;

    public class WeatherApiCacheInfo
    {
        public HttpResponse<WeatherApiResultModel> WeatherApiResult { get; set; }
        public DateTime CachedAtDateTime { get; set; }
    }
}
