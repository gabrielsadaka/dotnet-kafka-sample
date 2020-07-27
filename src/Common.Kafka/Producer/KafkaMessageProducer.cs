using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using Newtonsoft.Json;

namespace Common.Kafka.Producer
{
    public class KafkaMessageProducer : IMessageProducer
    {
        private readonly IKafkaProducerBuilder _kafkaProducerBuilder;

        public KafkaMessageProducer(IKafkaProducerBuilder kafkaProducerBuilder)
        {
            _kafkaProducerBuilder = kafkaProducerBuilder;
        }

        public async Task ProduceAsync(IMessage message, CancellationToken cancellationToken)
        {
            using (var producer = _kafkaProducerBuilder.Build())
            {
                message.Header.Type = message.GetType().AssemblyQualifiedName;
                var serialisedMessage = JsonConvert.SerializeObject(message);
                var topic = Attribute.GetCustomAttributes(message.GetType())
                    .OfType<MessageTopicAttribute>()
                    .Single()
                    .Topic;
                var producedMessage = new Message<string, string> {Key = message.Key, Value = serialisedMessage};

                await producer.ProduceAsync(topic, producedMessage, cancellationToken);
            }
        }
    }
}