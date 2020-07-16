using Confluent.Kafka;

namespace ProducerWorker.Infrastructure.Messaging
{
    public interface IMessageProducerBuilder
    {
        IProducer<string, string> Build();
    }
}