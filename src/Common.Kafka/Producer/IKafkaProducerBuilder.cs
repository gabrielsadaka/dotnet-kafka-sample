using Confluent.Kafka;

namespace Common.Kafka.Producer
{
    public interface IKafkaProducerBuilder
    {
        IProducer<string, string> Build();
    }
}