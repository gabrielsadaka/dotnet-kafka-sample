using System.Threading;

namespace Common.Kafka.Consumer
{
    public interface IKafkaMessageConsumer
    {
        void StartConsuming(CancellationToken cancellationToken);
    }

    public interface IKafkaMessageConsumer<TMessage> : IKafkaMessageConsumer
        where TMessage : IMessage
    {
    }
}