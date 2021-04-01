namespace SprinklerAgent.Weather
{
    using System.Threading.Tasks;
    using Models;

    public interface IWeatherApiClient
    {
        Task<HttpResponse<WeatherApiResultModel>> GetWeatherByUSZip(string zipCode);
    }
}
