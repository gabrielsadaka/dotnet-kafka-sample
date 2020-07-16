using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ProducerWorker.Infrastructure.Messaging
{
    public class MessageProducer : IMessageProducer
    {
        private readonly IMessageProducerBuilder _messageProducerBuilder;

        public MessageProducer(IMessageProducerBuilder messageProducerBuilder)
        {
            _messageProducerBuilder = messageProducerBuilder;
        }

        public async Task ProduceAsync(IMessage message, CancellationToken cancellationToken)
        {
            using (var producer = _messageProducerBuilder.Build())
            {
                var serialisedMessage = JObject.FromObject(message).ToString(Formatting.None);

                await producer.ProduceAsync(message.GetTopic(),
                    new Message<string, string> {Key = message.Key, Value = serialisedMessage}, cancellationToken);

                producer.Flush(cancellationToken);
            }
        }
    }
}