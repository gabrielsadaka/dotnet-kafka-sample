using Confluent.Kafka;

namespace Common.Kafka.Consumer
{
    public interface IKafkaConsumerBuilder
    {
        IConsumer<string, string> Build();
    }
}