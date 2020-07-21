using System.Threading;
using System.Threading.Tasks;

namespace Common.Kafka.Producer
{
    public interface IMessageProducer
    {
        Task ProduceAsync(IMessage message, CancellationToken cancellationToken);
    }
}