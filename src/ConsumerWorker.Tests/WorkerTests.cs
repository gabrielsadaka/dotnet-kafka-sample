using System.Threading;
using System.Threading.Tasks;
using Common.Kafka.Consumer;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace ConsumerWorker.Tests
{
    public class WorkerTests
    {
        [Fact]
        public async Task ExecuteAsyncShouldStartKafkaMessageConsumers()
        {
            var stubLogger = Mock.Of<ILogger<Worker>>();
            var mockKafkaMessageConsumerStarter = new Mock<IKafkaMessageConsumerStarter>();
            var cancellationTokenSource = new CancellationTokenSource();

            var sut = new Worker(stubLogger, mockKafkaMessageConsumerStarter.Object);
            await sut.StartAsync(cancellationTokenSource.Token);
            cancellationTokenSource.Cancel();

            mockKafkaMessageConsumerStarter.Verify(x => x.StartConsumers(It.IsAny<CancellationToken>()));
        }
    }
}