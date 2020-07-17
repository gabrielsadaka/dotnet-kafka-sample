using Confluent.Kafka;

namespace ProducerWorker.Infrastructure.Messaging
{
    public interface IKafkaProducerBuilder
    {
        IProducer<string, string> Build();
    }
}