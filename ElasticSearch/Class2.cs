using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ElasticSearch
{
    internal sealed class FieldConverter : JsonConverter<Field>
    {
        private readonly IElasticsearchClientSettings _settings;

        public FieldConverter(IElasticsearchClientSettings settings) => _settings = settings;

        public override Field? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            // Omitted for brevity
        }

        public override void Write(Utf8JsonWriter writer, Field value, JsonSerializerOptions options)
        {
            if (value is null)
            {
                writer.WriteNullValue();
                return;
            }

            var fieldName = _settings.Inferrer.Field(value);

            if (string.IsNullOrEmpty(value.Format))
            {
                writer.WriteStringValue(fieldName);
            }
            else
            {
                writer.WriteStartObject();
                writer.WritePropertyName("field");
                writer.WriteStringValue(fieldName);
                writer.WritePropertyName("format");
                writer.WriteStringValue(value.Format);
                writer.WriteEndObject();
            }
        }

        public DefaultRequestResponseSerializer(IElasticsearchClientSettings settings)
        {
            Options = new JsonSerializerOptions
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                IncludeFields = true,
                Converters =
            {
                new FieldConverter(settings),
                // Many other converters
            },
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            _settings = settings;
        }
    }
}
