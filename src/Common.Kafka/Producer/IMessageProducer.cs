using System.Threading;
using System.Threading.Tasks;

namespace Common.Kafka.Producer
{
    public interface IMessageProducer
    {
        Task ProduceAsync(string key, IMessage message, CancellationToken cancellationToken);
    }
}