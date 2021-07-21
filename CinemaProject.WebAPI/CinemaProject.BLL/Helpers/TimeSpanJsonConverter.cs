﻿using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CinemaProject.BLL.Helpers
{
    public class TimeSpanJsonConverter : JsonConverter<TimeSpan>
    {
        public override TimeSpan Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            long ticks = 0;
            var startDepth = reader.CurrentDepth;
            if (reader.TokenType == JsonTokenType.StartObject)
            {
                string propertyName = null;
                while (reader.Read())
                {
                    switch (reader.TokenType)
                    {
                        case JsonTokenType.EndObject when reader.CurrentDepth == startDepth:
                            return TimeSpan.FromTicks(ticks);
                        case JsonTokenType.PropertyName:
                            propertyName = reader.GetString();
                            break;
                    }
                    if (!string.IsNullOrWhiteSpace(propertyName) && propertyName.Equals("Ticks") && reader.TokenType == JsonTokenType.Number) ticks = reader.GetInt64();
                }
            }
            else if (reader.TokenType == JsonTokenType.Number) return TimeSpan.FromTicks(reader.GetInt64());
            return TimeSpan.Zero;
        }

        public override void Write(Utf8JsonWriter writer, TimeSpan value, JsonSerializerOptions options)
        {
            writer.WriteNumberValue(value.Ticks);
        }
    }
}
