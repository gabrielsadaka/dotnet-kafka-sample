using System.Threading;

namespace Common.Kafka.Consumer
{
    public interface IKafkaMessageConsumerManager
    {
        void StartConsumers(CancellationToken cancellationToken);
    }
}