using System.Threading;

namespace Common.Kafka.Consumer
{
    public interface IKafkaMessageConsumerStarter
    {
        void StartConsumers(CancellationToken cancellationToken);
    }
}