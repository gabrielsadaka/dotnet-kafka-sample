using System.Threading;
using System.Threading.Tasks;
using Common.Kafka;

namespace ProducerWorker.Infrastructure.Messaging
{
    public interface IMessageProducer
    {
        Task ProduceAsync(IMessage message, CancellationToken cancellationToken);
    }
}