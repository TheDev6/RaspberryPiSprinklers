namespace SprinklerAgent.Weather
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Configuration;
    using Models;
    using Newtonsoft.Json;

    public class WeatherApiClient : IWeatherApiClient
    {
        private readonly HttpClient _http;
        private readonly string _versionUrlSegment = "/data/2.5";
        private readonly string _apiKey;

        public WeatherApiClient(IConfig config)
        {
            _http = new HttpClient(new HttpClientHandler())
            {
                BaseAddress = new Uri(config.WeatherApiUrl())
            };
            _apiKey = config.WeatherApiKey();
        }

        public async Task<HttpResponse<WeatherApiResultModel>> GetWeatherByUSZip(string zipCode)
        {
            var result = new HttpResponse<WeatherApiResultModel>();
            var response = await _http.GetAsync(requestUri: $"{_versionUrlSegment}/weather?zip={zipCode},us&appid={_apiKey}");
            result.ContentString = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                result.IsSuccess = true;
                result.Payload = JsonConvert.DeserializeObject<WeatherApiResultModel>(result.ContentString);
            }
            return result;
        }
    }
}
