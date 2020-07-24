using System.Threading;

namespace Common.Kafka.Consumer
{
    public interface IKafkaTopicMessageConsumer
    {
        void StartConsuming(string topic, CancellationToken cancellationToken);
    }
}