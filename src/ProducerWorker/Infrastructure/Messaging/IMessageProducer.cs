using System.Threading;
using System.Threading.Tasks;

namespace ProducerWorker.Infrastructure.Messaging
{
    public interface IMessageProducer
    {
        Task ProduceAsync(IMessage message, CancellationToken cancellationToken);
    }
}