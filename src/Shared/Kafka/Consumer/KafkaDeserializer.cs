using System;
using System.Text;
using Confluent.Kafka;
using Newtonsoft.Json;

namespace Shared.Kafka.Consumer
{
    internal sealed class KafkaDeserializer<T> : IDeserializer<T>
    {
        public T Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
        {
            var dataJsonString = Encoding.UTF8.GetString(data);

            // deserializing twice because of double serialization of event payload.
            var normalizedJsonString = JsonConvert.DeserializeObject<string>(dataJsonString);

            return JsonConvert.DeserializeObject<T>(normalizedJsonString);
        }
    }
}