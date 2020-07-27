using System;
using System.Linq;
using System.Text;
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

        public async Task ProduceAsync(string key, IMessage message, CancellationToken cancellationToken)
        {
            // TODO: cache kafka producer as singleton
            using (var producer = _kafkaProducerBuilder.Build())
            {
                var serialisedMessage = JsonConvert.SerializeObject(message);
                var topic = Attribute.GetCustomAttributes(message.GetType())
                    .OfType<MessageTopicAttribute>()
                    .Single()
                    .Topic;

                var messageType = message.GetType().AssemblyQualifiedName;
                var producedMessage = new Message<string, string>
                {
                    Key = key,
                    Value = serialisedMessage,
                    Headers = new Headers
                    {
                        {"message-type", Encoding.UTF8.GetBytes(messageType)}
                    }
                };

                await producer.ProduceAsync(topic, producedMessage, cancellationToken);
            }
        }
    }
}