namespace SprinklerApi.JsonExtenders
{
    using System;
    using Newtonsoft.Json;

    public class TimeSpanConverter : JsonConverter<TimeSpan>
    {
        public override void WriteJson(
            JsonWriter writer,
            TimeSpan value,
            JsonSerializer serializer)
        {
            writer.WriteValue(value.ToString());
        }

        public override TimeSpan ReadJson(
            JsonReader reader,
            Type objectType,
            TimeSpan existingValue,
            bool hasExistingValue,
            JsonSerializer serializer)
        {
            return TimeSpan.Parse(reader.ReadAsString());
        }
    }
}
