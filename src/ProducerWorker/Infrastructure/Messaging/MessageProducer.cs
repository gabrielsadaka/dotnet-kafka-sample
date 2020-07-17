using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ProducerWorker.Infrastructure.Messaging
{
    public class MessageProducer : IMessageProducer
    {
        private readonly IKafkaProducerBuilder _kafkaProducerBuilder;

        public MessageProducer(IKafkaProducerBuilder kafkaProducerBuilder)
        {
            _kafkaProducerBuilder = kafkaProducerBuilder;
        }

        public async Task ProduceAsync(IMessage message, CancellationToken cancellationToken)
        {
            using (var producer = _kafkaProducerBuilder.Build())
            {
                var serialisedMessage = JObject.FromObject(message).ToString(Formatting.None);

                await producer.ProduceAsync(message.GetTopic(),
                    new Message<string, string> {Key = message.Key, Value = serialisedMessage}, cancellationToken);

                producer.Flush(cancellationToken);
            }
        }
    }
}