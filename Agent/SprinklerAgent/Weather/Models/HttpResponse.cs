namespace SprinklerAgent.Weather.Models
{
    public class HttpResponse<T> where T : new()
    {
        public bool IsSuccess { get; set; }
        public string ContentString { get; set; }
        public T Payload { get; set; }
    }
}
