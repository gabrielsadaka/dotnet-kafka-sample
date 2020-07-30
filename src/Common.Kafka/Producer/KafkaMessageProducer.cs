using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using Newtonsoft.Json;

namespace Common.Kafka.Producer
{
    public class KafkaMessageProducer : IMessageProducer, IDisposable
    {
        private readonly Lazy<IProducer<string, string>> _cachedProducer;

        public KafkaMessageProducer(IKafkaProducerBuilder kafkaProducerBuilder)
        {
            _cachedProducer = new Lazy<IProducer<string, string>>(() => kafkaProducerBuilder.Build());
        }

        public void Dispose()
        {
            if (_cachedProducer.IsValueCreated) _cachedProducer.Value.Dispose();
        }

        public async Task ProduceAsync(string key, IMessage message, CancellationToken cancellationToken)
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

            await _cachedProducer.Value.ProduceAsync(topic, producedMessage, cancellationToken);
        }
    }
}