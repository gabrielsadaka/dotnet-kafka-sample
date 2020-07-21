using System.Threading;
using System.Threading.Tasks;
using Common.Kafka;
using Confluent.Kafka;
using Newtonsoft.Json;

namespace ProducerWorker.Infrastructure.Messaging
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
                var serialisedMessage = JsonConvert.SerializeObject(message);

                await producer.ProduceAsync(message.Header.GetTopic(),
                    new Message<string, string> {Key = message.Key, Value = serialisedMessage}, cancellationToken);

                producer.Flush(cancellationToken);
            }
        }
    }
}