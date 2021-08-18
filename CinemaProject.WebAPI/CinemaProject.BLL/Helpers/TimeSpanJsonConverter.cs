using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CinemaProject.BLL.Helpers
{
    public class TimeSpanJsonConverter : JsonConverter<TimeSpan>
    {
        public const string TimeSpanFormatString = @"d\.hh\:mm\:ss\.FFFF";

        public override TimeSpan Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            TimeSpan parsedTimeSpan;
            TimeSpan.TryParseExact(reader.GetString(), TimeSpanFormatString, null, out parsedTimeSpan);
            return parsedTimeSpan;
        }

        public override void Write(Utf8JsonWriter writer, TimeSpan value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}
