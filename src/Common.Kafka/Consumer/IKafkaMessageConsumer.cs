using System.Threading;

namespace Common.Kafka.Consumer
{
    public interface IKafkaMessageConsumer<TMessage>
        where TMessage : IMessage
    {
        void StartConsuming(CancellationToken cancellationToken);
    }
}